using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VAction : MonoBehaviour
{
    protected string TAG;
    public bool inspect;

    protected List<CommandAction> cmds = new List<CommandAction>();
    protected Dictionary<int, System.Action> actions = new Dictionary<int, System.Action>();
    protected PlayerNavMesh playerNavMesh;
    protected bool audioPlayed = false;
    protected bool reachedDestination = false;
    protected virtual string  InspectAudioFileName { get;} = "";
    protected virtual string LateReplyAudioFileName { get; } = "";
    public void TriggerAction(int commandId)
    {
        Debug.Log("Action Triggered! " + TAG);
        VAction action = GameObject.FindWithTag(TAG).GetComponent<VAction>();
        action.actions[commandId]();
    }

    void Awake() {
        TAG = gameObject.tag;
        playerNavMesh = GameObject.Find("PlayerCapsule").GetComponent<PlayerNavMesh>();
    }

    public void OnReachedTarget()
    {
        reachedDestination = true;
    }
        public void ReadyThenInspect() {
        PlayerNavMesh.OnReachedTargetDelegate onReachedTarget = new PlayerNavMesh.OnReachedTargetDelegate(OnReachedTarget);
        playerNavMesh.GoToTarget(gameObject, onReachedTarget);
        Inspect();
    }

        public void Inspect()
    {
        Debug.Log("Inspecting Action! TAG: " + TAG);
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        Debug.Log("Playing Audio File: " + InspectAudioFileName);
        AudioClip DialogAudio = Resources.Load<AudioClip>(InspectAudioFileName);
        SoundManager.Instance.Play(DialogAudio);
        audioPlayed = true;
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;
        GameManager.LearnCommands(cmds);
    }

    IEnumerator LateInspectReply()
    {
        yield return new WaitForSeconds(3);
        AudioClip DialogAudio = Resources.Load<AudioClip>(LateReplyAudioFileName);
        SoundManager.Instance.Play(DialogAudio);
    }
    public void Start()
    {
       
        inspect = false;
        actions.Add(0, ReadyThenInspect);
        GameManager.commandActions.AddRange(cmds);
    }


    void Update() {
        if (!SoundManager.Instance.EffectsSource.isPlaying && audioPlayed && reachedDestination) {
            Debug.Log("Trash audio stopped");	
            StartCoroutine(LateInspectReply());
            audioPlayed = false;
            reachedDestination = false;
        }
    }

}