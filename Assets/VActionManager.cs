using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VActionManager : MonoBehaviour
{

    public VAction currentVAction { get; set; }
    public bool audioPlayed { get; set; } = false;
    public AudioActionType currentAudioActionType;

    //OnAudioQueueEnd

    public static VActionManager Instance = null;
    void Awake()
    {
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
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentVAction == null || audioPlayed == false) return;
        if (!SoundManager.Instance.EffectsSource.isPlaying)
        {
            audioPlayed = false;
            switch (currentAudioActionType)
            {
                case AudioActionType.Initial:
                    currentVAction.OnInitialAudioEnd();
                    break;
                case AudioActionType.FollowUp:
                    currentVAction.OnFollowUpEnd();
                    break;
                case AudioActionType.Action:
                    currentVAction.OnActionEnd();
                    break;
                case AudioActionType.NoAccess:
                    currentVAction.OnNoAccessEnd();
                    break;
                default:
                    break;
            }

        }

    }
}
