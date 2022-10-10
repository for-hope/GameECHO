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
    public VoiceProcessor voiceProcessor;
    private GameObject micOff;
    private AudioClip micOffClip;
    private PerformanceLogger performanceLogger;
    public static VoiceInputHandler Instance;

    public GameObject VoiceRecognitionObject;

    void Start()
    {
        //set InitialSilenceTimeoutSeconds to infinity so that the dictation recognizer doesn't stop listening
        var canvas = GameObject.Find("Canvas");
        micOn = canvas.transform.Find("MicOn").gameObject;
        micOff = canvas.transform.Find("MicOff").gameObject;
        micOnClip = Resources.Load<AudioClip>("Sounds/UI/start-mic");
        micOffClip = Resources.Load<AudioClip>("Sounds/UI/stop-mic");
        GameManager.isVoiceInteractionEnabled = false;
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

        if (GameManager.Instance.voiceInteractionEnabled && !micOn.activeSelf)
        {
            SoundManager.Instance.PlayUI(micOnClip);
            micOn.SetActive(true);
            micOff.SetActive(false);
        }
        else if (!GameManager.Instance.voiceInteractionEnabled && !micOff.activeSelf)
        {
            SoundManager.Instance.PlayUI(micOffClip);
            micOn.SetActive(false);
            micOff.SetActive(true);
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (GameManager.Instance.voiceInteractionEnabled)
            {
                Debug.Log("Stopping Dictation");
                DisableRecognizer();

            }
            else
            {
                Debug.Log("Starting Dictation");
                EnableRecognizer();

            }
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            var d = DictationInputManager.Instance.gameObject;
            Destroy(d);
            var go = new GameObject("DictationInputManager");
            go.AddComponent<DictationInputManager>();
        }

    }




    private bool ProcessIntroCommands(string text)
    {
        text = text.ToLower();
        if (text == "okay" || text == "ok") text = "okay";
        var activeIntroCommand = IntroManager.Instance.activeIntroCommand;
        if (activeIntroCommand == null) return false;
        if (text.Contains(activeIntroCommand.playerResponse.ToLower().ToCharArray()[0].ToString()))
        {
            IntroManager.Instance.NextCommand();
            return true;
        }

        //if (IntroManager.Instance.isActiveAndEnabled) StartCoroutine(DictationInputManager.StartRecording());
        return false;
    }

    private bool ProcessCommandActions(string text, ScopeFilter scopeFilter, int idx)
    {
        List<CommandAction> cmdActions = GameManager.commandActions;
        for (int i = 0; i < cmdActions.Count; i++)
        {
            if (text.ToLower().Contains(cmdActions[i].phrase.ToLower()))
            {
                GameManager.TriggerAction(cmdActions[i].id, cmdActions[i].context);
                return true;
            }
            if (idx == 0) scopeFilter.AddToFilter(cmdActions[i], text);
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
        if (GameManager.Instance.scopeOnlyPrediction) return scopeFilter.BestScoreCommand().Key;
        Debug.Log("=> Best Score Command: " + bestCommand.phrase + " with score: " + bestCommandWithScore.Value);
        return bestCommand;
    }

    public void DisableRecognizer(bool setFlag = true)
    {
        Debug.Log("Disabling Dictation");
        if (setFlag) GameManager.Instance.voiceInteractionEnabled = false;
        voiceProcessor.StopRecording();
    }

    public void EnableRecognizer(bool setFlag = true)
    {
        Debug.Log("Enabling Dictation");
        if (setFlag) GameManager.Instance.voiceInteractionEnabled = true;

        //if (voiceProcessor.IsRecording) return;
        voiceProcessor.StartRecording();
    }

    public void ProcessEnd()
    {
        EnableRecognizer();

    }
    private IEnumerator ProcessResult(List<string> texts)
    {
        DisableRecognizer();
        //Initilize Filters.
        var envFilter = new EnvironmentFilter();
        var contextFilter = new ContextFilter();
        var scopeFilter = new ScopeFilter();
        CommandLog commandLog = new CommandLog(texts[0]);
        string text = texts[0];
        bool broken = false;
        for (int i = 0; i < texts.Count; i++)
        {
            text = texts[i];
            Debug.Log("Dictation result: " + text);
            //Check if text is an intro command.
            var processIntro = ProcessIntroCommands(text);
           

            if (processIntro || (IntroManager.Instance.isIntroActive && texts.Count == i + 1))
            {
                Debug.Log("Intro Command " + texts.Count + " " + i + " " + processIntro);
                if (texts.Count == i + 1 && !processIntro) EnableRecognizer();
                yield break;
            }
            //process text by checking if it is a command action.
            if (ProcessCommandActions(text, scopeFilter, i))
            {
                commandLog = new CommandLog(text);
                commandLog.isCorrectlyRecognized = true;
                performanceLogger.AddToCommandLogs(commandLog);
                broken = true;
                yield break;
            }
        }
        if (broken) yield break;
        if (IntroManager.Instance.isIntroActive) yield break;
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




    public void RaiseDictationResult(List<string> texts)
    {
        if (texts[0] == "" || texts.Count == 0 || (!IntroManager.Instance.isIntroActive && !texts[0].Any(x => System.Char.IsWhiteSpace(x)))) return;
        for (int i = 0; i < texts.Count; i++)
        {
            Debug.Log(i + " Dictation result: " + texts[i]);
        }

        StartCoroutine(ProcessResult(texts));
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