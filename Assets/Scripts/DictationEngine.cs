using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class DictationEngine : MonoBehaviour
{
    protected DictationRecognizer dictationRecognizer;
    private bool dictationStarted = false;
    void Start()
    {
        Debug.Log("DictationEngine Start");
        StartDictationEngine();
    }

    void Update()
    {
        if (!GameManager.isVoiceInteractionEnabled && dictationStarted)
        {
            Debug.Log("DictationEngine Stopping...");
            CloseDictationEngine();
        }
        else if (GameManager.isVoiceInteractionEnabled && !dictationStarted )
        {
            Debug.Log("DictationEngine Starting...");
            StartDictationEngine();
        }


    }
    private void DictationRecognizer_OnDictationHypothesis(string text)
    {
        //Debug.Log("Dictation hypothesis: " + text);
        //Debug.Log(LevenshteinDistance.Compute("Hello world", text));
    }
    private void DictationRecognizer_OnDictationComplete(DictationCompletionCause completionCause)
    {
        switch (completionCause)
        {
            case DictationCompletionCause.TimeoutExceeded:
            case DictationCompletionCause.PauseLimitExceeded:
            case DictationCompletionCause.Canceled:
            case DictationCompletionCause.Complete:
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.UnknownError:
            case DictationCompletionCause.AudioQualityFailure:
            case DictationCompletionCause.MicrophoneUnavailable:
            case DictationCompletionCause.NetworkFailure:
                // Error
                CloseDictationEngine();
                break;
        }
    }
    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {

        Debug.Log("Dictation result: " + text + " with confidence: " + confidence);
        List<CommandAction> cmdActions = GameManager.commandActions;
        ScopeFilter scopeFilter = new ScopeFilter();

        bool cmdFound = false;
        for (int i = 0; i < cmdActions.Count; i++)
        {
            if (cmdActions[i].phrase.ToLower() == text.ToLower())
            {
                GameManager.TriggerAction(cmdActions[i].id, cmdActions[i].context);
                cmdFound = true;
                break;
            }
            scopeFilter.Filter(cmdActions[i], text);
        }

        if (cmdFound) return;
        IDictionary<CommandAction, int> scopeFilteredCommands = scopeFilter.filteredCommands;
        //predict the command
        Debug.Log("Command not found, Predicting...");
        if (scopeFilteredCommands.Count == 0)
        {
            Debug.Log("Command is not in the scope of the available commands");
            return;
        }
        float startTime = Time.realtimeSinceStartup;
        var bestScore = scopeFilter.BestScoreCommand();
        Debug.Log("[BEST GUESS BASED ON STRING COMP] : " + bestScore.Key.phrase + " with score: " + bestScore.Value);



        var scopeFilteredCommandsList = scopeFilteredCommands.Keys.ToList();
        var cmdBasedOnRay = EnvironmentFilter.filteredByEnv(scopeFilteredCommandsList);
        var cmdsBasedOnFrame = EnvironmentFilter.filterByFrameEnv(scopeFilteredCommandsList);
        IDictionary<CommandAction, int> cmdsBasedOnContext = AvailabilityFilter.filterByContext(scopeFilteredCommandsList);
        IDictionary<CommandAction, int> finalCommandsScores = new Dictionary<CommandAction, int>();
        Debug.Log("---- Calculating final score ----");
        foreach (KeyValuePair<CommandAction, int> commandScore in scopeFilteredCommands)
        {
            CommandAction possibleCmd = commandScore.Key;
            int score = 0; // MAX SCORE = 100
            //Environment Score MAX = 30
            if (cmdBasedOnRay.Contains(possibleCmd)) score += 20;
            var uniqueObjectsOnFrame = cmdsBasedOnFrame.Values.Distinct().Count();
            if (cmdsBasedOnFrame.Keys.ToList().Contains(possibleCmd)) score += (10 / uniqueObjectsOnFrame);
            //Context Score MAX = 30
            if (cmdsBasedOnContext.Keys.Contains(possibleCmd)) score += cmdsBasedOnContext[possibleCmd];
            //Scope Score MAX = 40
            score += Convert.ToInt32((ScopeFilter.MIN_SCOPE_FILTER_SCORE - commandScore.Value + 1) * 4);
            //Debug.Log("Score after str " + score);

            finalCommandsScores.Add(new KeyValuePair<CommandAction, int>(possibleCmd, score));
        }
        var maxCommand = finalCommandsScores.OrderByDescending(x => x.Value).First();
        CommandAction finalCommand = maxCommand.Key;
        Debug.Log("========> Final Command: " + finalCommand.phrase + " with score: " + maxCommand.Value);
        float endTime = Time.realtimeSinceStartup;
        Debug.Log("Time taken to predict: " + (endTime - startTime) + " seconds");

        GameManager.TriggerAction(finalCommand.id, finalCommand.context);

    }
    private void DictationRecognizer_OnDictationError(string error, int hresult)
    {
        Debug.Log("Dictation error: " + error);
    }
    private void OnApplicationQuit()
    {
        CloseDictationEngine();
    }
    private void StartDictationEngine()
    {
        dictationStarted = true;
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.Start();
    }
    private void CloseDictationEngine()
    {
        dictationStarted = false;
        if (dictationRecognizer != null)
        {
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_OnDictationHypothesis;
            dictationRecognizer.DictationComplete -= DictationRecognizer_OnDictationComplete;
            dictationRecognizer.DictationResult -= DictationRecognizer_OnDictationResult;
            dictationRecognizer.DictationError -= DictationRecognizer_OnDictationError;
            if (dictationRecognizer.Status == SpeechSystemStatus.Running)
            {
                dictationRecognizer.Stop();
            }
            dictationRecognizer.Dispose();
        }
    }
}