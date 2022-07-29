using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceCommands : MonoBehaviour
{

    private KeywordRecognizer KeywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>(); 
    void Start()
    {
        actions.Add("forward", Forward);
        actions.Add("back", Back);
        actions.Add("up", Up);
        actions.Add("down", Down);

        KeywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        KeywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        KeywordRecognizer.Start();
 
        Debug.Log("Script started!");
    

    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        transform.Translate(1, 0, 0);
        
    }

    private void Back()
    {
        transform.Translate(-1, 0, 0);

    }

    private void Up()
    {
        transform.Translate(0, 1, 0);

    }
    
    private void Down()
    {
        transform.Translate(0, -1, 0);
    }

}
