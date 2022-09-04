using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashcanAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/aXX";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/aXX";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the trashcan");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What's inside the trashcan", "What's inside");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Empty the trashcan", "Empty");


        actions.Add(new ActionFlow(1, WhatsInside, "Sounds/trashcan-2a", "Sounds/whats-inside-trashcan", "Sounds/trashcan-2b"));
        actions.Add(new ActionFlow(2, EmptyTrashcan, "Sounds/trashcan-3a", "", ""));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void WhatsInside()
    {
        Debug.Log("What's inside trashcan");
        cmds[1].isUsedOnce = true;

    }

    public void EmptyTrashcan()
    {
        Debug.Log("trying to move the bed");
        cmds[2].isUsedOnce = true;

    }




}