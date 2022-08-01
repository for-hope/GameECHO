using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashAction : VAction
{

    protected override string InspectAudioFileName { get; } = "Sounds/a13";

    protected override string LateReplyAudioFileName { get; } = "Sounds/a14";
    public bool move;
    public bool search;


    public void Move()
    {
        Debug.Log("Moving trash");
        //TODO voice answer
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a16");
        SoundManager.Instance.Play(DialogAudio);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 5, gameObject.transform.position.y, gameObject.transform.position.z);
        move = true;

    }

    public void Search()
    {
        Debug.Log("Searching inside trash");
        search = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a15");
        SoundManager.Instance.Play(DialogAudio);
    }



    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the trash");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Move the trash");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Search inside the trash");

        move = false;
        search = false;


        actions.Add(1, Move);
        actions.Add(2, Search);

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);

        base.Start();

    }

}
