using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketsAction : VAction
{

    protected override string InitialInspectAudioFN
    {
        get => "Sounds/a7";
    }

    protected override string FollowUpInspectAudioFN
    {
        get => "Sounds/a8";
    }

    protected override string ActionEffectInspectAudioFN
    {
        get => "Sounds/inspect-bucket";
    }


    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the buckets");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Move the buckets", "Move");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Search the buckets", "Search");
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        actions.Add(1, Move);
        actions.Add(2, Search);
        base.Start();
    }



    public void Move()
    {
        Debug.Log("Moving Bucket");
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 5, gameObject.transform.position.y, gameObject.transform.position.z);
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a37");
        SoundManager.Instance.Play(DialogAudio);
    }

    public void Search()
    {
        Debug.Log("Searching Bucket");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a38");
        SoundManager.Instance.Play(DialogAudio);
    }

}
