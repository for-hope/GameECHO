using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeDoorAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/orange-door-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/orange-door-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the orange door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Open the orange door", "Open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the orange door", "Break");


        actions.Add(new ActionFlow(1, OpenOrangeDoor, "Sounds/orange-door-2a", "Sounds/locked-door", "Sounds/orange-door-2b"));
        actions.Add(new ActionFlow(2, BreakOrangeDoor, "Sounds/orange-door-3a", "", ""));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void OpenOrangeDoor()
    {
        //TODO: Cut the wires near the orange door
        cmds[1].isUsedOnce = true;

    }

    public void BreakOrangeDoor()
    {

        //TODO: Cut the wires near the white door and LOSE
        cmds[2].isUsedOnce = true;

    }




}
