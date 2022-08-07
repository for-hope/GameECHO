using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a13";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a14";


    public void Move()
    {
        Debug.Log("Moving trash");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a16");
        SoundManager.Instance.Play(DialogAudio);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 5, gameObject.transform.position.y, gameObject.transform.position.z);

        cmds[1].isUsedOnce = true;
    }

    public void Search()
    {
        Debug.Log("Searching inside trash");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a15");
        SoundManager.Instance.Play(DialogAudio);
        cmds[2].isUsedOnce = true;
    }



    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the trash");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Move the trash", "Move");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Look inside the trash", "Look inside");


        actions.Add(1, Move);
        actions.Add(2, Search);

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);

        base.Start();

    }

}
