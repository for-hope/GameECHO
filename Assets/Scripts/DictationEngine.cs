using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class DictationEngine : MonoBehaviour
{
    protected DictationRecognizer dictationRecognizer;

    private GameObject micOn;
    private GameObject micOff;
    private PerformanceLogger performanceLogger;

    void Start()
    {
        //set InitialSilenceTimeoutSeconds to infinity so that the dictation recognizer doesn't stop listening

        micOn = GameObject.Find("MicOn");
        micOff = GameObject.Find("MicOff");
        Debug.Log("DictationEngine Start");
        GameManager.isVoiceInteractionEnabled = true;
        StartDictationEngine();
        performanceLogger = new PerformanceLogger();
    }

    void Awake()
    {
        //dont destroy
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {

        if (GameManager.isVoiceInteractionEnabled)
        {
            micOn.SetActive(true);
            micOff.SetActive(false);
        }
        else
        {
            micOn.SetActive(false);
            micOff.SetActive(true);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("r key pressed: Restarting");
            dictationRecognizer.Stop();
            dictationRecognizer.Dispose();
            dictationRecognizer.Start();
        }
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("STATUS: " + dictationRecognizer.Status);
        }

        if (dictationRecognizer.Status == SpeechSystemStatus.Failed)
        {
            Debug.Log("DictationRecognizer Failed Status: " + dictationRecognizer.Status);
        }

    }
    private void DictationRecognizer_OnDictationHypothesis(string text)
    {
        if (!GameManager.isVoiceInteractionEnabled) GameManager.Instance.ShowMutedFeedback();
        Debug.Log("Dictation hypothesis: " + text);
        //Debug.Log(LevenshteinDistance.Compute("Hello world", text));
    }
    private void DictationRecognizer_OnDictationComplete(DictationCompletionCause completionCause)
    {
        switch (completionCause)
        {
            case DictationCompletionCause.TimeoutExceeded:
                Debug.Log("Dictation has timed out.");
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;

            case DictationCompletionCause.PauseLimitExceeded:
                Debug.LogError("Dictation pause limit exceeded");
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.Canceled:
                Debug.LogError("Dictation cancelled");
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.Complete:
                // Restart required
                Debug.Log("Dictation complete");
                if (dictationRecognizer != null) dictationRecognizer.Start();
                // CloseDictationEngine();
                // StartDictationEngine();
                break;
            case DictationCompletionCause.UnknownError:
                Debug.LogError("Dictation unknown error");
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.AudioQualityFailure:
                Debug.LogError("Dictation audio quality failure");
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
            case DictationCompletionCause.MicrophoneUnavailable:
                Debug.LogError("Dictation microphone unavailable");
                break;
            case DictationCompletionCause.NetworkFailure:
                // Error
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                Debug.LogError("Dictation error: " + completionCause);
                break;
            default:
                Debug.LogError("Dictation error: " + completionCause);
                // Restart required
                CloseDictationEngine();
                StartDictationEngine();
                break;
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

    // private bool processEndingConfirmation(string text)
    // {
    //     if (IntroManager.Instance.endingToConfirm == null) return false;
    //     if (text == "yes" || text == "yeah" || text == "yep" || text == "yup" || text == "sure" || text == "okay" || text == "ok")
    //     {
    //         IntroManager.Instance.EndingConfirmation(true);
    //         return true;
    //     } else if (text == "no" || text == "nope" || text == "nah" || text == "no thanks")
    //     {
    //         IntroManager.Instance.EndingConfirmation(false);
    //         return true;
    //     }
    //     return false;
    // }
    private void DictationRecognizer_OnDictationResult(string text, ConfidenceLevel confidence)
    {

        if (!GameManager.isVoiceInteractionEnabled)
        {
            Debug.Log("Dictation is not enabled");
            return;
        }
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
    private void DictationRecognizer_OnDictationError(string error, int hresult)
    {
        Debug.Log("Dictation error: " + error);
    }
    private void OnApplicationQuit()
    {
        performanceLogger.SaveToDisk();
        dictationRecognizer.Dispose();

        CloseDictationEngine();
    }
    private void StartDictationEngine()
    {
        Debug.Log("Starting Dictation Engine");
        GameManager.isVoiceInteractionEnabled = true;
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.Start();
    }
    private void CloseDictationEngine()
    {
        Debug.Log("Closing Dictation Engine");
        GameManager.isVoiceInteractionEnabled = false;
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
        }
    }
}