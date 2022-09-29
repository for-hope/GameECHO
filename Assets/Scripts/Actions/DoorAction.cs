using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a9";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a10";


    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the door", "Try to Open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the door with the pipe", "Break with pipe", Visibility.HIDDEN);
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        actions.Add(new ActionFlow(1, TryToOpen, "", "Sounds/locked-door", "Sounds/a11"));
        actions.Add(new ActionFlow(2, BreakWithPipe, "", "Sounds/slam-door", "Sounds/a12"));
        base.Start();
    }



    public void TryToOpen()
    {
        
        cmds[1].isUsedOnce = true;
    }

    public void BreakWithPipe()
    {

        cmds[2].isUsedOnce = true;
    }

}
