using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0; i < result.Phrases.Length; i++)
        {
            if (i > 0)
            {
                resultText += ", ";
            }

            resultText += result.Phrases[i].Text;
        }
        Debug.Log(result.Partial ? "Partial" : "Final");
        Debug.Log("RAW RESULT" + resultText);
        VoiceInputHandler.Instance.RaiseDictationResult(result.Phrases[0].Text);
        resultText += "\n";
    }
}
