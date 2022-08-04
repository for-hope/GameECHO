using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a25";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a26";
    public bool lookUnder;
    public bool tryMove;




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the bed");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Look under the bed");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Try to move the bed");
        lookUnder = false;
        tryMove = false;

        actions.Add(1, LookUnder);
        actions.Add(2, TryMove);
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void LookUnder()
    {
        Debug.Log("looking under the bed");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a28");
        SoundManager.Instance.Play(DialogAudio);
        lookUnder = true;
    }

    public void TryMove()
    {
        Debug.Log("trying to move the bed");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a27");
        SoundManager.Instance.Play(DialogAudio);
        tryMove = true;
    }




}
