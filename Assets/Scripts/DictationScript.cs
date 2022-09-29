using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class DictationScript : MonoBehaviour
{
    private GameObject canvas;
    private GameObject micOn;
    private GameObject micOff;
    private PerformanceLogger performanceLogger;
    private DictationRecognizer m_DictationRecognizer;



    void Start()
    {
        canvas = GameObject.Find("Canvas");
        micOn = canvas.transform.Find("MicOn").gameObject;
        micOff = canvas.transform.Find("MicOff").gameObject;
        GameManager.isVoiceInteractionEnabled = true;
        performanceLogger = new PerformanceLogger();
        m_DictationRecognizer = new DictationRecognizer();
        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            DictationRecognizer_OnDictationResult(text, confidence);
        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
            if (!GameManager.isVoiceInteractionEnabled) GameManager.Instance.ShowMutedFeedback();

        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
            {
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);

            }
            Restart();
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start();
    }

    void Restart()
    {
        //restart gameobject
        GameManager.isVoiceInteractionEnabled = false;
        m_DictationRecognizer.Stop();
        m_DictationRecognizer.Dispose();
        m_DictationRecognizer = null;
        Destroy(this.gameObject);
        GameObject go = new GameObject("DictationManager");
        go.AddComponent<DictationScript>();
    }

    void OnDestroy()
    {
        if (m_DictationRecognizer != null)
        {
            m_DictationRecognizer.Stop();
            m_DictationRecognizer.Dispose();
            m_DictationRecognizer = null;
        }
    }

    void Update()
    {

        if (GameManager.isVoiceInteractionEnabled && m_DictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            micOn.SetActive(true);
            micOff.SetActive(false);
        }
        else
        {
            micOn.SetActive(false);
            micOff.SetActive(true);
        }

    }


    private bool processIntroCommands(string text)
    {
        if (text == "okay" || text == "ok") text = "okay";
        var activeIntroCommand = IntroManager.Instance.activeIntroCommand;
        if (activeIntroCommand == null) return false;
        if (activeIntroCommand.playerResponse.ToLower() == text)
        {
            IntroManager.Instance.NextCommand();
            return true;
        }
        return false;

    }

    private bool processCmdActions(string text, ScopeFilter scopeFilter)
    {
        List<CommandAction> cmdActions = GameManager.commandActions;
        //   Debug.Log("Checking cmds against " + cmdActions.Count + " commands");
        for (int i = 0; i < cmdActions.Count; i++)
        {
            // Debug.Log("Checking " + cmdActions[i].phrase.ToLower() + " with " + text);
            if (cmdActions[i].phrase.ToLower() == text.ToLower())
            {
                // Debug.Log("Matched " + cmdActions[i].phrase.ToLower() + " with id and context" + cmdActions[i].id + " " + cmdActions[i].context);
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


    private CommandAction CalculateBestCommand(string text, ScopeFilter scopeFilter, EnvironmentFilter envFilter, ContextFilter contextFilter, ref CommandLog commandLog)
    {

        var scopeFilteredCommandsList = possibleCommands(text, scopeFilter);
        //If no commands are found, return.
        if (scopeFilteredCommandsList == null) return null;
        //process commands through the filters
        commandLog.numberOfCommandsConsidered = scopeFilteredCommandsList.Count;
        commandLog.commandBasedOnScopeFilter = scopeFilter.BestScoreCommand().Key;
        commandLog.commandPredictedScopeFilterScore = scopeFilter.BestScoreCommand().Value;
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
        commandLog.commandPredictedContextFilterScore = contextFilter.getCommandScore(bestCommandWithScore.Key);
        commandLog.commandPredictedEnvironmentFilterScore = envFilter.getCommandScore(bestCommandWithScore.Key);
        commandLog.commandPredicted = bestCommand;
        commandLog.commandPredictedScore = bestCommandWithScore.Value;
        Debug.Log("=> Best Score Command: " + bestCommand.phrase + " with score: " + bestCommandWithScore.Value);
        return bestCommand;
    }


    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {

        Debug.Log("Dictation result: " + text + " with confidence: " + confidence);
        //Check if text is an intro command.
        //if (processIntroCommands(text)) return;
        if (processIntroCommands(text.ToLower()) || IntroManager.Instance.isIntroActive) return;
        //Initilize Filters.
        var envFilter = new EnvironmentFilter();
        var contextFilter = new ContextFilter();
        var scopeFilter = new ScopeFilter();
        CommandLog commandLog = new CommandLog(text, confidence.ToString());
        //process text by checking if it is a command action.
        if (processCmdActions(text, scopeFilter))
        {
            commandLog.isCorrectlyRecognized = true;
            performanceLogger.AddToCommandLogs(commandLog);
            return;
        }

        //!COMMAND NOT FOUND, PREDICTING...
        var startTime = Time.realtimeSinceStartup;
        commandLog.isCorrectlyRecognized = false;
        Debug.Log("Command not found, Predicting...");
        //Get possible commands from the scope filter and ignore commands with low string distance to the text.
        var predictedCommand = CalculateBestCommand(text, scopeFilter, envFilter, contextFilter, ref commandLog);
        if (predictedCommand == null)
        {
            Debug.Log("No command found");
            commandLog.passedMinimumScopeFilter = false;
            performanceLogger.AddToCommandLogs(commandLog);
            DefaultVoiceFallback.Instance.PlayFallbackVoice();
            //Trigger default feedback
            return;
        }
        float endTime = Time.realtimeSinceStartup;
        commandLog.passedMinimumScopeFilter = true;
        commandLog.timeTakenToPredict = endTime - startTime;
        Debug.Log("Time taken to predict: " + (endTime - startTime) + " seconds");
        performanceLogger.AddToCommandLogs(commandLog);
        GameManager.TriggerAction(predictedCommand.id, predictedCommand.context);
    }
    private void OnApplicationQuit()
    {
        performanceLogger.SaveToDisk();
        m_DictationRecognizer.Stop();
        m_DictationRecognizer.Dispose();
    }
}