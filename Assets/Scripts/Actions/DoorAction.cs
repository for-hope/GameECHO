using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : VAction
{

    protected override string InitialInspectAudioFN { get;} = "Sounds/a9";
 
    protected override string FollowUpInspectAudioFN  { get;} = "Sounds/a10";

    public bool tryOpen;
    public bool tryBreak;

    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the door");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the door with the pipe");
        tryOpen = false;
        tryBreak = false;
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        actions.Add(1, TryOpen);
        actions.Add(2, TryBreak);
        base.Start();
    }



    public void TryOpen()
    {
        Debug.Log("Trying to open the door");
        tryOpen = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a11");
        SoundManager.Instance.Play(DialogAudio);
    }

    public void TryBreak()
    {
        Debug.Log("Trying to break the door");
        tryBreak = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a12");
        SoundManager.Instance.Play(DialogAudio);
        //TODO voice answer
    }

}
