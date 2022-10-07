using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioActionType
{
    Initial,
    Action,
    FollowUp,
    NoAccess

}
public class AudioAction
{
    public string TAG { get; set; }
    public AudioActionType actionType;
    public string audioFN;

    public int actionId = -1;
    public bool noAnim = false;

    // public AudioAction(AudioActionType actionType, string audioFN)
    // {
    //     this.actionType = actionType;
    //     this.audioFN = audioFN;
    // }

    public AudioAction(AudioActionType actionType, string audioFN, int actionId, bool noAnim = false)
    {
        this.actionType = actionType;
        this.audioFN = audioFN;
        this.actionId = actionId;
        this.noAnim = noAnim;

    }
}

public class ActionFlow
{
    public int commandID;
    public System.Action action;
    public string startAudioFilename;
    public string actionAudioFilename;
    public string followUpAudioFilename;




    public System.Action endAction;

    public List<int> accessCmds;
    public string noAccessAudioFilename;

    public Queue<AudioAction> audioQueue = new Queue<AudioAction>();
    public Queue<AudioAction> noAccessAudioQueue = new Queue<AudioAction>();


    public ActionFlow(int id, System.Action action, string startAudioFilename, string actionAudioFilename, string followUpAudioFilename, List<int> accessCmds = null, string noAccessAudioFilename = "", System.Action endAction = null, bool noAnim = false)
    {
        this.commandID = id;
        this.action = action;
        this.startAudioFilename = startAudioFilename;
        this.actionAudioFilename = actionAudioFilename;
        this.followUpAudioFilename = followUpAudioFilename;
        this.noAccessAudioFilename = noAccessAudioFilename;
        this.endAction = endAction;

        AudioClip startAudioClip = Resources.Load<AudioClip>(this.startAudioFilename);
        AudioClip actionAudioClip = Resources.Load<AudioClip>(this.actionAudioFilename);
        AudioClip responseAudioClip = Resources.Load<AudioClip>(this.followUpAudioFilename);
        AudioClip noAccessAudioClip = Resources.Load<AudioClip>(this.noAccessAudioFilename);
        this.accessCmds = accessCmds;
        audioQueue.Enqueue(new AudioAction(AudioActionType.Initial, this.startAudioFilename, this.commandID));
        audioQueue.Enqueue(new AudioAction(AudioActionType.Action, this.actionAudioFilename, this.commandID, noAnim: noAnim));
        audioQueue.Enqueue(new AudioAction(AudioActionType.FollowUp, this.followUpAudioFilename, this.commandID));
        noAccessAudioQueue.Enqueue(new AudioAction(AudioActionType.NoAccess, this.noAccessAudioFilename, this.commandID));

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

    private List<string> alreadyUsedCommandSounds = new List<string>(){
        "Sounds/already-did-it",
        "Sounds/already-done-that",
        "Sounds/i-did-that-already"
    };
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

        //VAction action = GameManager.Instance.FindGameObjectWithVAction(TAG).GetComponent<VAction>();
        ActionFlow actionFlow = this.actions.Find(x => x.commandID == commandId);
        var cmd = cmds.Find(x => x.id == actionFlow.commandID);
        VActionManager.Instance.currentVAction = this;
        if (cmd.isUsedOnce)
        {
            Queue<AudioAction> audioQueue = new Queue<AudioAction>();
            string randomVoiceLine = alreadyUsedCommandSounds[UnityEngine.Random.Range(0, alreadyUsedCommandSounds.Count)];
            audioQueue.Enqueue(new AudioAction(AudioActionType.Initial, randomVoiceLine, actionFlow.commandID));
            StartCoroutine(PlayAudioOnQueue(audioQueue));
            return;
        }

        GoToTargetThen();
        doAction(actionFlow);
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
        Debug.Log("Navmesh: Reached target");
        reachedDestination = true;
    }
    public void GoToTargetThen()
    {
        PlayerNavMesh.OnReachedTargetDelegate onReachedTarget = new PlayerNavMesh.OnReachedTargetDelegate(OnReachedTarget);
        playerNavMesh.GoToTarget(gameObject, onReachedTarget);

    }

    public virtual void Inspect()
    {
        inspect = true;
        GameManager.RevealHiddenCommandsOfAction(cmds);
    }

    IEnumerator PlayAudioOnQueue(Queue<AudioAction> audioActions)
    {
        Debug.Log("PlayAudioOnQueue");

        currentAudioAction = audioActions.Dequeue();
        Debug.Log("PlayAudioOnQueue: " + currentAudioAction.audioFN);
        VActionManager.Instance.currentAudioActionType = currentAudioAction.actionType;
        if (currentAudioAction.actionType == AudioActionType.NoAccess) audioActions.Enqueue(currentAudioAction);
        CommandAction cmd = cmds.Find(x => x.id == currentAudioAction.actionId);
        ActionFlow actionFlow = actions.Find(x => x.commandID == currentAudioAction.actionId);

        if (currentAudioAction.actionType == AudioActionType.Action && !cmd.noAnimation) GameManager.Instance.putDownCamera(true);
        if (currentAudioAction.actionType == AudioActionType.Action && cmd.noAnimation && currentAudioAction.actionId >= 0) actionFlow.action();
        if (currentAudioAction.actionType != AudioActionType.Initial) yield return new WaitForSeconds(1);

        AudioClip DialogAudio = Resources.Load<AudioClip>(currentAudioAction.audioFN);
        SoundManager.Instance.Play(DialogAudio);
        audioPlayed = true;
        VActionManager.Instance.audioPlayed = true;

    }

    public void Start()
    {
        inspect = false;
        actions.Add(new ActionFlow(0, Inspect, InitialInspectAudioFN, inspectAudioFN, FollowUpInspectAudioFN));
        GameManager.Instance.AddActionCommands(cmds);
    }

    protected void doAction(ActionFlow actionFlow)
    {

        var hasAccess = actionFlow.accessCmds == null;
        if (!hasAccess)
        {
            foreach (int accessCmdId in actionFlow.accessCmds)
            {

                if (cmds[accessCmdId].isUsedOnce)
                {
                    hasAccess = true;
                    actionFlow.accessCmds = null;
                    break;
                }
            }

        }

        if (!hasAccess)
        {
            StartCoroutine(PlayAudioOnQueue(actionFlow.noAccessAudioQueue));
            return;
        }

        cmds[actionFlow.commandID].isUsedOnce = true;
        StartCoroutine(PlayAudioOnQueue(actionFlow.audioQueue));
    }

    private void NextInAudioQueue()
    {
        ActionFlow actionFlow = actions.Find(x => x.commandID == currentAudioAction.actionId);
        if (actionFlow.audioQueue.Count == 0) OnAudioQueueEnded();
        else if (currentAudioAction.actionType != AudioActionType.NoAccess) StartCoroutine(PlayAudioOnQueue(actionFlow.audioQueue));
        Debug.Log("ActionType: " + currentAudioAction);
        Debug.Log("VoiceInputHandler: " + VoiceInputHandler.Instance);
        if (currentAudioAction.actionType == AudioActionType.NoAccess) VoiceInputHandler.Instance.EnableRecognizer();

    }

    public void OnAudioQueueEnded()
    {
        Debug.Log("Audio queue ended");
        ActionFlow actionFlow = actions.Find(x => x.commandID == currentAudioAction.actionId);
        if (actionFlow.endAction != null)
        {
            actionFlow.endAction();
            actionFlow.endAction = null;
        }
        VoiceInputHandler.Instance.EnableRecognizer();
        currentAudioAction = null;
    }

    public void OnInitialAudioEnd()
    {
        Debug.Log("Initial audio ended");
        NextInAudioQueue();

    }

    public void OnActionEnd()
    {
        Debug.Log("Action ended");
        ActionFlow actionFlow = actions.Find(x => x.commandID == currentAudioAction.actionId);
        GameManager.Instance.putDownCamera(false);
        if (currentAudioAction.actionId >= 0) actionFlow.action();
        NextInAudioQueue();
    }
    public void OnFollowUpEnd()
    {
        Debug.Log("Follow up ended");
        NextInAudioQueue();
    }

    public void OnNoAccessEnd()
    {
        Debug.Log("No access ended");
        NextInAudioQueue();
    }



}