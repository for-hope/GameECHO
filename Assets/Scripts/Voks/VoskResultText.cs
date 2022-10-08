using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VoskResultText : MonoBehaviour
{
    public VoskSpeechToText VoskSpeechToText;
    //public Text ResultText;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
        //Debug.Log(obj);
        string resultText = "";
        var result = new RecognitionResult(obj);
        var phrases = new List<string>();
        for (int i = 0; i < result.Phrases.Length; i++)
        {
            phrases.Add(result.Phrases[i].Text);
            if (i > 0)
            {
                resultText += ", ";
            }

            resultText += result.Phrases[i].Text;
        }
        Debug.Log(result.Partial ? "Partial" : "Final");
        Debug.Log("RAW RESULT" + resultText);
       
        VoiceInputHandler.Instance.RaiseDictationResult(phrases);
        resultText += "\n";
    }
}
