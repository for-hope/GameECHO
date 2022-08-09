using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // protected override string ActionEffectInspectAudioFN
    // {
    //     get => "Sounds/inspect-bucket";
    // }


    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the buckets");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Move the buckets", "Move");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Search the buckets", "Search");
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        actions.Add(new ActionFlow(1, Move, "Sounds/a37", "Sounds/move-buckets", ""));
        actions.Add(new ActionFlow(2, Search, "Sounds/a38", "Sounds/search-buckets", ""));
        base.Start();
    }



    public void Move()
    {
        Debug.Log("Moving Bucket");
        GameObject movedBuckets = Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x.name == "Moved_Buckets").FirstOrDefault();
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        movedBuckets.SetActive(true);
        cmds[1].isUsedOnce = true;

    }

    public void Search()
    {
        Debug.Log("Searching Bucket");
        cmds[2].isUsedOnce = true;
    }

}
