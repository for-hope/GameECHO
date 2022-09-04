using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBathroomAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/small-bathroom-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/small-bathroom-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the small bathroom");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Check for exists in the small bathroom", "Check Exits");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Close the door of the small bathroom", "Close Door");


        actions.Add(new ActionFlow(1, CheckExits, "Sounds/small-bathroom-2a", "Sounds/check-exits", "Sounds/small-bathroom-2b"));
        actions.Add(new ActionFlow(2, CloseDoor, "Sounds/small-bathroom-3a", "Sounds/close-door", "Sounds/small-bathroom-3b"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void CheckExits()
    {
        cmds[1].isUsedOnce = true;

    }

    public void CloseDoor()
    {

        cmds[2].isUsedOnce = true;
        //TODO: Close the door of the small bathroom

    }




}
