using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class DictationEngine : MonoBehaviour
{
    protected DictationRecognizer dictationRecognizer;
    private bool dictationStarted = false;
    private GameObject micOn;
    private GameObject micOff;
    void Start()
    {
        micOn = GameObject.Find("MicOn");
        micOff = GameObject.Find("MicOff");
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
        else if (GameManager.isVoiceInteractionEnabled && !dictationStarted)
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


    private bool processIntroCommands(string text)
    {
        if (text.ToLower() == "okay" || text.ToLower() == "ok")
        {
            GameManager.Instance.IntroScreen.SetActive(false);
            return true;
        }
        return false;
    }

    private bool processCmdActions(string text, ScopeFilter scopeFilter)
    {
        List<CommandAction> cmdActions = GameManager.commandActions;
        Debug.Log("Checking cmds against " + cmdActions.Count + " commands");
        for (int i = 0; i < cmdActions.Count; i++)
        {
            Debug.Log("Checking " + cmdActions[i].phrase.ToLower() + " with " + text);
            if (cmdActions[i].phrase.ToLower() == text.ToLower())
            {
                Debug.Log("Matched " + cmdActions[i].phrase.ToLower() + " with id and context" + cmdActions[i].id + " " + cmdActions[i].context);
                GameManager.TriggerAction(cmdActions[i].id, cmdActions[i].context);
                return true;
            }
            scopeFilter.AddToFilter(cmdActions[i], text);
        }
        return false;
    }

    private List<CommandAction> possibleCommands(string text, ScopeFilter scopeFilter)
    {

        if (GameManager.Instance.predictorEnabled)
        {

            IDictionary<CommandAction, int> scopeFilteredCommands = scopeFilter.filteredCommands;
            if (scopeFilteredCommands.Count == 0) return null; //DEFAULT FEEDBACK
            var bestScore = scopeFilter.BestScoreCommand();
            Debug.Log("[BEST GUESS BASED ON STRING COMP] : " + bestScore.Key.phrase + " with score: " + bestScore.Value);
            return scopeFilteredCommands.Keys.ToList();

        }
        return null;
    }


    private CommandAction CalculateBestCommand(string text, ScopeFilter scopeFilter, EnvironmentFilter envFilter, ContextFilter contextFilter)
    {
        var scopeFilteredCommandsList = possibleCommands(text, scopeFilter);
        //If no commands are found, return.
        if (scopeFilteredCommandsList == null) return null;
        //process commands through the filters
        envFilter.AddToFilter(scopeFilteredCommandsList);
        contextFilter.AddToFilter(scopeFilteredCommandsList);


        IDictionary<CommandAction, int> finalCommandsScores = new Dictionary<CommandAction, int>();
        Debug.Log("---- Calculating final score ----");
        foreach (CommandAction possibleCmd in scopeFilteredCommandsList)
        {
            int score = 0; // MAX SCORE = 100
            //Environment Score MAX = 30
            score += envFilter.CalculateScore(possibleCmd);
            //Context Score MAX = 30
            score += contextFilter.CalculateScore(possibleCmd);
            //Scope Score MAX = 40
            score += scopeFilter.CalculateScore(possibleCmd);

            finalCommandsScores.Add(new KeyValuePair<CommandAction, int>(possibleCmd, score));
        }
        var bestCommandWithScore = finalCommandsScores.OrderByDescending(x => x.Value).First();

        CommandAction bestCommand = bestCommandWithScore.Key;
        Debug.Log("========> Best Score Command: " + bestCommand.phrase + " with score: " + bestCommandWithScore.Value);

        return bestCommand;
    }

    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {

        Debug.Log("Dictation result: " + text + " with confidence: " + confidence);
        //Check if text is an intro command.
        if (processIntroCommands(text)) return;
        //Initilize Filters.
        var envFilter = new EnvironmentFilter();
        var contextFilter = new ContextFilter();
        var scopeFilter = new ScopeFilter();
        //process text by checking if it is a command action.
        if (processCmdActions(text, scopeFilter)) return;

        //!COMMAND NOT FOUND, PREDICTING...
        var startTime = Time.realtimeSinceStartup;
        Debug.Log("Command not found, Predicting...");
        //Get possible commands from the scope filter and ignore commands with low string distance to the text.
        var predictedCommand = CalculateBestCommand(text, scopeFilter, envFilter, contextFilter);
        if (predictedCommand == null)
        {
            Debug.Log("No command found");
            //Trigger default feedback
            return;
        }
        float endTime = Time.realtimeSinceStartup;
        Debug.Log("Time taken to predict: " + (endTime - startTime) + " seconds");
        GameManager.TriggerAction(predictedCommand.id, predictedCommand.context);
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
        micOn.SetActive(true);
        micOff.SetActive(false);
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
        micOn.SetActive(false);
        micOff.SetActive(true);
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