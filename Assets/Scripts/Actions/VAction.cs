using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioActionType
{
    Initial,
    Action,
    FollowUp
}
public class AudioAction
{
    public AudioActionType actionType;
    public string audioFN;

    public AudioAction(AudioActionType actionType, string audioFN)
    {
        this.actionType = actionType;
        this.audioFN = audioFN;
    }
}
public class VAction : MonoBehaviour
{

    protected string TAG;
    public bool inspect;

    protected List<CommandAction> cmds = new List<CommandAction>();
    protected Dictionary<int, System.Action> actions = new Dictionary<int, System.Action>();
    protected PlayerNavMesh playerNavMesh;
    protected bool audioPlayed = false;
    protected bool reachedDestination = false;
    protected virtual string InitialInspectAudioFN { get; } = "";

    protected virtual string ActionEffectInspectAudioFN { get; } = "";
    protected virtual string FollowUpInspectAudioFN { get; } = "";

    private Queue<AudioAction> queuedAudioFiles = new Queue<AudioAction>();

    private AudioAction currentAudioAction = null;
    public void TriggerAction(int commandId)
    {
        VAction action = GameObject.FindWithTag(TAG).GetComponent<VAction>();
        action.actions[commandId]();
    }

    void Awake()
    {
        TAG = gameObject.tag;
        playerNavMesh = GameObject.Find("PlayerCapsule").GetComponent<PlayerNavMesh>();

    }

    public void OnReachedTarget()
    {
        reachedDestination = true;
    }
    public void ReadyThenInspect()
    {
        PlayerNavMesh.OnReachedTargetDelegate onReachedTarget = new PlayerNavMesh.OnReachedTargetDelegate(OnReachedTarget);
        playerNavMesh.GoToTarget(gameObject, onReachedTarget);
        Inspect();
    }

    public void Inspect()
    {
        Debug.Log("Inspecting Action! TAG: " + TAG);
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        audioPlayed = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>(InitialInspectAudioFN);
        SoundManager.Instance.Play(DialogAudio);
        audioPlayed = false;
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;
        GameManager.RevealHiddenCommandsOfAction(cmds);
    }

    IEnumerator PlayAudioOnQueue()
    {
        currentAudioAction = queuedAudioFiles.Dequeue();
        if (currentAudioAction.actionType == AudioActionType.Action)
        {
            GameManager.Instance.putDownCamera(true);
        }
        yield return new WaitForSeconds(1);

        AudioClip DialogAudio = Resources.Load<AudioClip>(currentAudioAction.audioFN);
        SoundManager.Instance.Play(DialogAudio);
        audioPlayed = false;


    }
    public void Start()
    {
        inspect = false;
        queuedAudioFiles.Enqueue(new AudioAction(AudioActionType.Action, ActionEffectInspectAudioFN));
        queuedAudioFiles.Enqueue(new AudioAction(AudioActionType.FollowUp, FollowUpInspectAudioFN));


        actions.Add(0, ReadyThenInspect);
        GameManager.commandActions.AddRange(cmds);
    }


    void Update()
    {
        if (!SoundManager.Instance.EffectsSource.isPlaying && !audioPlayed && reachedDestination)
        {
            if (currentAudioAction != null && currentAudioAction.actionType == AudioActionType.Action)
            {
                Debug.Log("Action Audio Finished");
                GameManager.Instance.putDownCamera(false);
            }
            if (queuedAudioFiles.Count != 0)
            {
                audioPlayed = true;
                StartCoroutine(PlayAudioOnQueue());

            }
        }
    }

}