using System;
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
    public string TAG { get; set; }
    public AudioActionType actionType;
    public string audioFN;

    public int actionId = -1;

    // public AudioAction(AudioActionType actionType, string audioFN)
    // {
    //     this.actionType = actionType;
    //     this.audioFN = audioFN;
    // }

    public AudioAction(AudioActionType actionType, string audioFN, int actionId)
    {
        this.actionType = actionType;
        this.audioFN = audioFN;
        this.actionId = actionId;

    }
}

public class ActionFlow
{
    public int commandID;
    public System.Action action;
    public string startAudioFilename;
    public string actionAudioFilename;
    public string followUpAudioFilename;

    public Queue<AudioAction> audioQueue = new Queue<AudioAction>();

    public ActionFlow(int id, System.Action action, string startAudioFilename, string actionAudioFilename, string followUpAudioFilename)
    {
        this.commandID = id;
        this.action = action;
        this.startAudioFilename = startAudioFilename;
        this.actionAudioFilename = actionAudioFilename;
        this.followUpAudioFilename = followUpAudioFilename;
        AudioClip startAudioClip = Resources.Load<AudioClip>(this.startAudioFilename);
        AudioClip actionAudioClip = Resources.Load<AudioClip>(this.actionAudioFilename);
        AudioClip responseAudioClip = Resources.Load<AudioClip>(this.followUpAudioFilename);
        audioQueue.Enqueue(new AudioAction(AudioActionType.Initial, this.startAudioFilename, this.commandID));
        audioQueue.Enqueue(new AudioAction(AudioActionType.Action, this.actionAudioFilename, this.commandID));
        audioQueue.Enqueue(new AudioAction(AudioActionType.FollowUp, this.followUpAudioFilename, this.commandID));

    }

    public override bool Equals(object obj)
    {
        return obj is ActionFlow flow &&
               commandID == flow.commandID &&
               EqualityComparer<Action>.Default.Equals(action, flow.action) &&
               startAudioFilename == flow.startAudioFilename &&
               actionAudioFilename == flow.actionAudioFilename &&
               followUpAudioFilename == flow.followUpAudioFilename;
    }

    public override int GetHashCode()
    {
        int hashCode = -37182905;
        hashCode = hashCode * -1521134295 + commandID.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Action>.Default.GetHashCode(action);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(startAudioFilename);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(actionAudioFilename);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(followUpAudioFilename);
        return hashCode;
    }
}

public class VAction : MonoBehaviour
{

    protected string TAG;
    public bool inspect;

    protected List<CommandAction> cmds = new List<CommandAction>();
    protected List<ActionFlow> actions = new List<ActionFlow>();
    protected PlayerNavMesh playerNavMesh;
    protected bool audioPlayed = false;
    protected bool reachedDestination = false;
    protected virtual string InitialInspectAudioFN { get; } = "";

    //protected virtual string ActionEffectInspectAudioFN { get; } = "";
    protected virtual string FollowUpInspectAudioFN { get; } = "";
    private string inspectAudioFN;

    //private Queue<AudioAction> queuedAudioFiles = new Queue<AudioAction>();

    private AudioAction currentAudioAction = null;
    public void TriggerAction(int commandId)
    {
        VAction action = GameObject.FindWithTag(TAG).GetComponent<VAction>();
        GoToTargetThen();
        doAction(action.actions.Find(x => x.commandID == commandId));
        //action.actions.Find(x => x.commandID == commandId).action();
    }

    public List<CommandAction> GetVisibleCommands()
    {
        List<CommandAction> visibleCommands = new List<CommandAction>();
        foreach (CommandAction cmd in cmds)
        {
            if (cmd.visibility == Visibility.VISIBLE)
            {
                visibleCommands.Add(cmd);
            }
        }
        return visibleCommands;
    }

    public List<CommandAction> GetInvisibleCommands()
    {
        List<CommandAction> invisibleCommands = new List<CommandAction>();
        foreach (CommandAction cmd in cmds)
        {
            if (cmd.visibility == Visibility.INVISIBLE)
            {
                invisibleCommands.Add(cmd);
            }
        }
        return invisibleCommands;
    }

    public List<CommandAction> GetHiddenCommands()
    {
        List<CommandAction> hiddenCommands = new List<CommandAction>();
        foreach (CommandAction cmd in cmds)
        {
            if (cmd.visibility == Visibility.HIDDEN)
            {
                hiddenCommands.Add(cmd);
            }
        }
        return hiddenCommands;
    }

    void Awake()
    {
        TAG = gameObject.tag;
        inspectAudioFN = "Sounds/inspect-" + TAG.ToLower();
        playerNavMesh = GameObject.Find("PlayerCapsule").GetComponent<PlayerNavMesh>();

    }

    public void OnReachedTarget()
    {
        reachedDestination = true;
    }
    public void GoToTargetThen()
    {
        PlayerNavMesh.OnReachedTargetDelegate onReachedTarget = new PlayerNavMesh.OnReachedTargetDelegate(OnReachedTarget);
        playerNavMesh.GoToTarget(gameObject, onReachedTarget);

    }

    public void Inspect()
    {
        //INSPECT
        Debug.Log("Inspected Action! TAG: " + TAG);
        inspect = true;
        GameManager.RevealHiddenCommandsOfAction(cmds);
    }

    IEnumerator PlayAudioOnQueue(Queue<AudioAction> audioActions)
    {
        audioPlayed = true;
        Debug.Log("Playing audio on q " + audioActions.Count);
        currentAudioAction = audioActions.Dequeue();
        if (currentAudioAction.actionType == AudioActionType.Action)
        {
            GameManager.Instance.putDownCamera(true);
        }
        if (currentAudioAction.actionType != AudioActionType.Initial) yield return new WaitForSeconds(1);

        AudioClip DialogAudio = Resources.Load<AudioClip>(currentAudioAction.audioFN);

        SoundManager.Instance.Play(DialogAudio);
        audioPlayed = false;


    }
    public void Start()
    {
        inspect = false;
        //queuedAudioFiles.Enqueue(new AudioAction(AudioActionType.Action, inspectAudioFN));
        //queuedAudioFiles.Enqueue(new AudioAction(AudioActionType.FollowUp, FollowUpInspectAudioFN));

        actions.Add(new ActionFlow(0, Inspect, InitialInspectAudioFN, inspectAudioFN, FollowUpInspectAudioFN));
        GameManager.commandActions.AddRange(cmds);
    }

    protected void doAction(ActionFlow actionFlow)
    {
        GameManager.isVoiceInteractionEnabled = false;
        Debug.Log("VOICE INTERACTION DISABLED");
        StartCoroutine(PlayAudioOnQueue(actionFlow.audioQueue));
    }


    void Update()
    {
        if (!SoundManager.Instance.EffectsSource.isPlaying && !audioPlayed && reachedDestination)
        {
            ActionFlow actionFlow = actions.Find(x => x.commandID == currentAudioAction.actionId);
            if (currentAudioAction != null && currentAudioAction.actionType == AudioActionType.Action)
            {
                Debug.Log("Action Audio Finished");
                GameManager.Instance.putDownCamera(false);
                if (currentAudioAction.actionId >= 0) actionFlow.action();
            }
            if (actionFlow.audioQueue.Count != 0)
            {
                StartCoroutine(PlayAudioOnQueue(actionFlow.audioQueue));
            } else if (!GameManager.isVoiceInteractionEnabled) {
                GameManager.isVoiceInteractionEnabled = true;
                Debug.Log("VOICE INTERACTION ENABLED");
            }
            
        }
    }

}