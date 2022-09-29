using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CommandLog
{
    public string outputText;
    public string confidenceLevel;
    public bool isCorrectlyRecognized;
    public bool passedMinimumScopeFilter;
    public CommandAction commandBasedOnScopeFilter;
    public CommandAction commandPredicted;
    public int commandPredictedScore = 0;
    public int commandPredictedScopeFilterScore = 0;
    public int commandPredictedContextFilterScore = 0;
    public int commandPredictedEnvironmentFilterScore = 0;
    public string predictedPhrase = "";

    public string predictedPhraseBasedOnScope = "";

    public float timeTakenToPredict = 0;

    public int numberOfCommandsConsidered;

    //constructor
    public CommandLog(string outputText, string confidenceLevel)
    {
        this.outputText = outputText;
        this.confidenceLevel = confidenceLevel;
    }




}
[System.Serializable]
public class PerformanceLogger
{
    public int totalVoiceInputs = 0;
    public int totalCorrectlyRecognized = 0;
    public int totalPassedMinimumScopeFilter = 0;
    public int totalCommandsPredicted = 0;


    public float averagePredictionScore = 0;
    public float averageScopeFilterScore = 0;
    public float averageContextFilterScore = 0;
    public float averageEnvironmentFilterScore = 0;

    public int totalCommandsPredictedCByScopeOnly = 0;

    [SerializeField]
    public List<CommandLog> commandLogs = new List<CommandLog>();
    public List<CommandLog> commandLogsPredicted = new List<CommandLog>();


    public void AddToCommandLogs(CommandLog commandLog)
    {
        commandLogs.Add(commandLog);
        totalVoiceInputs++;
        if (commandLog.isCorrectlyRecognized) totalCorrectlyRecognized++;
        if (commandLog.passedMinimumScopeFilter) totalPassedMinimumScopeFilter++;
        if (commandLog.commandPredicted != null)
        {
            totalCommandsPredicted++;
            if (commandLog.commandPredicted == commandLog.commandBasedOnScopeFilter) totalCommandsPredictedCByScopeOnly++;
            commandLog.predictedPhrase = commandLog.commandPredicted.phrase;
            commandLog.predictedPhraseBasedOnScope = commandLog.commandBasedOnScopeFilter.phrase;
            commandLogsPredicted.Add(commandLog);
        }
        SaveToDisk();
    }

    private void CalculateAverageScores()
    {
        averagePredictionScore = 0;
        averageScopeFilterScore = 0;
        averageContextFilterScore = 0;
        averageEnvironmentFilterScore = 0;
        List<CommandLog> predictedCommands = commandLogs.FindAll(x => x.commandPredicted != null);
        foreach (CommandLog cl in predictedCommands)
        {
            averagePredictionScore += cl.commandPredictedScore;
            averageScopeFilterScore += cl.commandPredictedScopeFilterScore;
            averageContextFilterScore += cl.commandPredictedContextFilterScore;
            averageEnvironmentFilterScore += cl.commandPredictedEnvironmentFilterScore;
        }
        averagePredictionScore /= predictedCommands.Count;
        averageScopeFilterScore /= predictedCommands.Count;
        averageContextFilterScore /= predictedCommands.Count;
        averageEnvironmentFilterScore /= predictedCommands.Count;

    }

    public void SaveToDisk()
    {
        //convert fields to json
        CalculateAverageScores();
        string json = JsonUtility.ToJson(this);
        //save or replace to disk
        System.IO.File.WriteAllText(Application.dataPath + "/PerformanceLog.json", json);


    }
}
