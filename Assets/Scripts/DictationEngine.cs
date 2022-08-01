using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class DictationEngine : MonoBehaviour
{
    protected DictationRecognizer dictationRecognizer;
    void Start()
    {
        Debug.Log("DictationEngine Start");
        StartDictationEngine();
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
        //Debug.Log("Dictation result: " + text);
        //if (CommandList.phrases.Contains(text.ToLower()))
        //{
        //    int id = 0;
        //    foreach (VoiceInteraction va in CommandList.commandsArray)
        //    {
        //        if (va.phrase == text.ToLower())
        //        {
        //            id = va.id;
        //        }
        //    }
        //    Debug.Log("ACTION OF ID : " + id);
        //} else
        //{

        //    List<int> av = AvailabilityFilter.filterByAv();
        //    List<int> ef = EnvFilter.filteredByEnv();

        //    List<int> maybeActions = new List<int>();
        //    for (int i = 0; i < av.Count; i++)
        //    {

        //        if (ef.Contains(av[i]))
        //        {
        //            //Debug.Log("[MAYBE1] ACTION OF ID : " + av[i]);
        //            maybeActions.Add(av[i]);
        //        }
        //    }
        //    int currentScore = 0;
        //    int lastScore = 100;
        //    int winnerAction = -1;
        //    string phrase = "";
        //    foreach (VoiceInteraction va in CommandList.commandsArray)
        //    {
        //        if (maybeActions.Contains(va.id))
        //        {
        //            //Debug.Log("ACTION OF ID : " + va.id);
        //            currentScore = LevenshteinDistance.Compute(va.phrase, text);
        //            if (currentScore < lastScore)
        //            {
        //                lastScore = currentScore;
        //                winnerAction = va.id;
        //                phrase = va.phrase;
        //            }

        //        }

        //    }
        //    Debug.Log("[FINAL] ACTION OF ID : " + winnerAction + " WITH SCORE : " + lastScore + " AND PHRASE : " + phrase);
        //}
        Debug.Log("Dictation result: " + text);
        List<CommandAction> cmdActions = GameManager.commandActions;
        IDictionary<CommandAction, int> commandStringScore = new Dictionary< CommandAction,int>();
        bool cmdFound = false;
        for (int i = 0; i < cmdActions.Count; i++)
        {
            if (cmdActions[i].phrase.ToLower() == text.ToLower())
            {
                GameManager.TriggerAction(cmdActions[i].id, cmdActions[i].context);
                cmdFound = true;
                break;
            }
            int score = LevenshteinDistance.Compute(cmdActions[i].phrase, text);
            commandStringScore.Add(new KeyValuePair<CommandAction, int>(cmdActions[i], score));
        }

        if (cmdFound) return;

        //predict the command
        Debug.Log("1st STEP" + commandStringScore.Values.Min());
        int max = commandStringScore.Values.Min();
        Debug.Log("MAX SCORE" + commandStringScore.First(x => x.Value == max).Key.phrase);
        if (commandStringScore.Values.Min() > 12) return;
        List<CommandAction> possibleCommands = new List<CommandAction>();
        List<int> possibleCommandsScores = new List<int>();
        foreach (var item in commandStringScore)
        {
            CommandAction cmdAction = item.Key;
            int score = item.Value;
            if (score <= 12)
            {
                possibleCommands.Add(cmdAction);
                possibleCommandsScores.Add(score);
            }
        }
        Debug.Log("2nd STEP");

        List<CommandAction> cmdBasedOnRay = EnvFilter.filteredByEnv(GameManager.commandActions);
        List<CommandAction> cmdsBasedOnFrame = EnvFilter.filterByFrameEnv(GameManager.commandActions);
        IDictionary<CommandAction, int> cmdsBasedOnContext = AvailabilityFilter.filterByGameState(GameManager.commandActions);
        Debug.Log("CMDS BASED ON RAY => " + cmdBasedOnRay.Count);
        foreach (CommandAction cmd in cmdsBasedOnFrame)
        {
            Debug.Log("CMD BASED ON FRAME " + cmd.phrase);
        }

        foreach (var cmd in cmdsBasedOnContext)
        {
            Debug.Log("CMD BASED ON Context " + cmd.Key.phrase + " With score " + cmd.Value);
        }
        IDictionary<CommandAction, int> finalScore = new Dictionary<CommandAction, int>();
        for (int i=0; i < possibleCommands.Count; i++)
        {
            CommandAction possibleCmd = possibleCommands[i];
            int score = 0;
            Debug.Log("Scoring cmd " + possibleCmd.phrase);
            if (cmdBasedOnRay.Contains(possibleCmd)) score += 20;
            Debug.Log("Score after ray " + score);
            if (cmdsBasedOnFrame.Contains(possibleCmd)) score += 10;
            Debug.Log("Score after frames " + score);
            if (cmdsBasedOnContext.Keys.Contains(possibleCmd)) score += ((cmdsBasedOnContext[possibleCmd] - 5) * 6);
            Debug.Log("Score after context " + score );

            score += Convert.ToInt32((101 - possibleCommandsScores[i] - 88) * 2.5);
            Debug.Log("Score after str " + score);

            finalScore.Add(new KeyValuePair<CommandAction, int>(possibleCmd, score));
        }

        Debug.Log("3rd STEP");

        int maxValue = finalScore.Values.Max();
        Debug.Log("MAx Value" + maxValue);
        CommandAction finalCommand =  finalScore.First(x => x.Value == maxValue).Key;
        Debug.Log("cmd id " + finalCommand.id);

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
        dictationRecognizer = new DictationRecognizer();
        dictationRecognizer.DictationHypothesis += DictationRecognizer_OnDictationHypothesis;
        dictationRecognizer.DictationResult += DictationRecognizer_OnDictationResult;
        dictationRecognizer.DictationComplete += DictationRecognizer_OnDictationComplete;
        dictationRecognizer.DictationError += DictationRecognizer_OnDictationError;
        dictationRecognizer.Start();
    }
    private void CloseDictationEngine()
    {
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