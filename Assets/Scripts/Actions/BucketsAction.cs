using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketsAction : VAction
{

    public bool move;
    public bool search;


    protected override string InspectAudioFileName
    {
        get => "Sounds/a7";
    }

    protected override string LateReplyAudioFileName  {
        get => "Sounds/a8";
    }



    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the buckets");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Move the buckets");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Search the buckets");
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        actions.Add(1, Move);
        actions.Add(2, Search);
        move = false;
        search = false;
        base.Start();
    }



    public void Move()
    {
        Debug.Log("Moving Bucket");
        //TODO voice answer 
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 5, gameObject.transform.position.y, gameObject.transform.position.z);
        move = true;
    }

    public void Search()
    {
        Debug.Log("Searching Bucket");
        search = true;
        //TODO voice answer
    }

}
