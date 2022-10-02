using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultVoiceFallback : MonoBehaviour
{
    //0.wav 1.wav 2.wav 3.wav
    string baseDir = "Sounds/Defaults/";
    int numberOfResponses = 4;
    bool inProgress = false;

    public static DefaultVoiceFallback Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!inProgress) return;
        if (!SoundManager.Instance.EffectsSource.isPlaying)
        {
            inProgress = false;
            //GameManager.isVoiceInteractionEnabled = true;
            StartCoroutine(DictationInputManager.StartRecording());
        }
    }
    private string RandomResponse()
    {
        int randomIndex = Random.Range(0, numberOfResponses);
        return baseDir + randomIndex.ToString();
    }

    public void PlayFallbackVoice()
    {
        //GameManager.isVoiceInteractionEnabled = false;
        inProgress = true;
        string path = RandomResponse();
        AudioClip clip = Resources.Load<AudioClip>(path);
        SoundManager.Instance.Play(clip);
    }
}
