using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.Collections;

public class VoiceInputHandler : MonoBehaviour
{

    private GameObject micOn;
    private AudioClip micOnClip;
    
    private GameObject micOff;
    private AudioClip micOffClip;
    private PerformanceLogger performanceLogger;
    public static VoiceInputHandler Instance;

    void Start()
    {
        //set InitialSilenceTimeoutSeconds to infinity so that the dictation recognizer doesn't stop listening
        var canvas = GameObject.Find("Canvas");
        micOn =  canvas.transform.Find("MicOn").gameObject;
        micOff = canvas.transform.Find("MicOff").gameObject;
        micOnClip = Resources.Load<AudioClip>("Sounds/UI/start-mic");
        micOffClip = Resources.Load<AudioClip>("Sounds/UI/stop-mic");
        //GameManager.isVoiceInteractionEnabled = true;
        performanceLogger = new PerformanceLogger();
    }

    void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {

        if (DictationInputManager.IsListening && !micOn.activeSelf)
        {
            SoundManager.Instance.Play(micOnClip);
            micOn.SetActive(true);
            micOff.SetActive(false);
        }
        else if (!DictationInputManager.IsListening && !micOff.activeSelf)
        {
            SoundManager.Instance.Play(micOffClip);
            micOn.SetActive(false);
            micOff.SetActive(true);
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (DictationInputManager.IsListening)
            {
                Debug.Log("Stopping Dictation");
                StartCoroutine(DictationInputManager.StopRecording());
            }
            else
            {
                Debug.Log("Starting Dictation");
                StartCoroutine(DictationInputManager.StartRecording());
            }
        }

    }

    private void Restart()
    {
        Debug.Log("Not Restarting");
        // if (dictationRecognizer == null) StartDictationEngine();
        // if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        // {
        //     dictationRecognizer.Stop();

        // }
        // dictationRecognizer.Start();

    }


    private bool ProcessIntroCommands(string text)
    {
        text = text.ToLower();
        if (text == "okay" || text == "ok") text = "okay";
        var activeIntroCommand = IntroManager.Instance.activeIntroCommand;
        if (activeIntroCommand == null) return false;
        if (activeIntroCommand.playerResponse.ToLower() == text)
        {
            IntroManager.Instance.NextCommand();
            return true;
        }
        if (IntroManager.Instance.isActiveAndEnabled) StartCoroutine(DictationInputManager.StartRecording());
        return false;
    }

    private bool ProcessCommandActions(string text, ScopeFilter scopeFilter)
    {
        List<CommandAction> cmdActions = GameManager.commandActions;
        for (int i = 0; i < cmdActions.Count; i++)
        {
            if (cmdActions[i].phrase.ToLower() == text.ToLower())
            {
                GameManager.TriggerAction(cmdActions[i].id, cmdActions[i].context);
                return true;
            }
            scopeFilter.AddToFilter(cmdActions[i], text);
        }
        return false;
    }

    private List<CommandAction> GetPossibleCommands(string text, ScopeFilter scopeFilter)
    {

        if (!GameManager.Instance.predictorEnabled) return null;
        IDictionary<CommandAction, int> scopeFilteredCommands = scopeFilter.filteredCommands;
        if (scopeFilteredCommands.Count == 0) return null; //DEFAULT FEEDBACK
        var bestScore = scopeFilter.BestScoreCommand();
        Debug.Log("[BEST GUESS BASED ON STRING COMP] : " + bestScore.Key.phrase + " with score: " + bestScore.Value);
        return scopeFilteredCommands.Keys.ToList();


    }


    private CommandAction CalculateBestCommand(string text, ScopeFilter scopeFilter, EnvironmentFilter envFilter, ContextFilter contextFilter, ref CommandLog commandLog)
    {

        var scopeFilteredCommandsList = GetPossibleCommands(text, scopeFilter);
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

    private bool Tester() {
        GameManager.TriggerAction(0, "TABLE");
        return true;
    }

    private IEnumerator ProcessResult(string text)
    {
        StartCoroutine(DictationInputManager.StopRecording());
        Debug.Log("Dictation result: " + text);
        //Check if text is an intro command.
        if (ProcessIntroCommands(text) || IntroManager.Instance.isIntroActive) yield break;
        if (Tester()) yield break;
        //Initilize Filters.
        var envFilter = new EnvironmentFilter();
        var contextFilter = new ContextFilter();
        var scopeFilter = new ScopeFilter();
        CommandLog commandLog = new CommandLog(text);
        //process text by checking if it is a command action.
        if (ProcessCommandActions(text, scopeFilter))
        {
            commandLog.isCorrectlyRecognized = true;
            performanceLogger.AddToCommandLogs(commandLog);
            yield break;
        }
        //!COMMAND NOT FOUND, PREDICTING...
        //var startTime = Time.realtimeSinceStartup;
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
            yield break;
        }
        //float endTime = Time.realtimeSinceStartup;
        commandLog.passedMinimumScopeFilter = true;
        //commandLog.timeTakenToPredict = endTime - startTime;
        //Debug.Log("Time taken to predict: " + (endTime - startTime) + " seconds");
        performanceLogger.AddToCommandLogs(commandLog);
        Debug.Log("Predicted command: " + predictedCommand.phrase);
        GameManager.TriggerAction(predictedCommand.id, predictedCommand.context);
    }

    public void RaiseDictationHypothesis(string text)
    {
        if (text == "") return;
        Debug.LogFormat("Dictation hypothesis: {0}", text);
        //ProcessCommandActions(text, new ScopeFilter());
    }




    public void RaiseDictationResult(string text)
    {
        if (text == "") return;
        Debug.LogFormat("Dictation result: {0}", text);
        StartCoroutine(ProcessResult(text));
    }

    public void RaiseDictationComplete(bool isError, string reason, string result, AudioClip clip)
    {
        Debug.LogFormat("Dictation complete: Error ? {0} reason? {1} result? {2} ", isError, reason, result);

    }

    public void RaiseDictationError(string errorText)
    {
        Debug.LogFormat(errorText);
    }


    public void SaveAndClose()
    {
        performanceLogger.SaveToDisk();
    }
    private void OnApplicationQuit()
    {
        SaveAndClose();
    }

}