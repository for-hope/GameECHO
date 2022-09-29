using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/clock-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/clock-1b";




    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the clock");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "How much time do you have left", "Time left?");
        actions.Add(new ActionFlow(1, TimeLeft, "Sounds/clock-2a", "", ""));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        base.Start();
    }




    private void TimeLeft()
    {
        cmds[1].isUsedOnce = true;
    }






}
