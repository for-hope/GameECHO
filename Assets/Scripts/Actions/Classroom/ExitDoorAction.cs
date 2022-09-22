using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/exit-door-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/exit-door-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the classroom door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Force open the classroom door with the broom", "Force Open", Visibility.HIDDEN);
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Exit the classroom from the exit door and run away", "Run!");



        actions.Add(new ActionFlow(1, ForceOpen, "Sounds/exit-door-2a", "Sounds/force-open-exit-door", "Sounds/exit-door-2b"));
        actions.Add(new ActionFlow(1, ForceOpen, "Sounds/exit-door-3a", "", ""));



        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);



        base.Start();
    }




    private void ForceOpen()
    {
        cmds[1].isUsedOnce = true;
    }

    private void RunAway() {
        cmds[2].isUsedOnce = true;
    }



}
