using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a25";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a26";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the bed");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Look under the bed", "Look Under");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Try to move the bed", "Try to Move");


        actions.Add(new ActionFlow(1, LookUnder, "", "Sounds/look-under-bed", "Sounds/a28"));
        actions.Add(new ActionFlow(2, TryToMove, "", "Sounds/move-bed", "Sounds/a27"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void LookUnder()
    {
        Debug.Log("looking under the bed");
        cmds[1].isUsedOnce = true;

    }

    public void TryToMove()
    {
        Debug.Log("trying to move the bed");
        cmds[2].isUsedOnce = true;

    }




}
