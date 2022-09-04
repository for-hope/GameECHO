using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDoorAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/white-door-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/white-door-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the white door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Open the white door", "Open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the white door", "Break");


        actions.Add(new ActionFlow(1, OpenWhiteDoor, "Sounds/white-door-2a", "Sounds/locked-door", "Sounds/white-door-2b"));
        actions.Add(new ActionFlow(2, BreakWhiteDoor, "Sounds/white-door-3a", "", ""));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void OpenWhiteDoor()
    {
        //TODO: Cut the wires near the orange door
        cmds[1].isUsedOnce = true;

    }

    public void BreakWhiteDoor()
    {

        //TODO: Cut the wires near the white door and LOSE
        cmds[2].isUsedOnce = true;

    }




}
