using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/alarm-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/alarm-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the alarm");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Dismantle the alarm", "Dismantle");



        actions.Add(new ActionFlow(1, Dismantle, "Sounds/alarm-2a", "", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);



        base.Start();
    }

    private void InspectAlarm()
    {
        cmds[0].isUsedOnce = true;
        WiresAction.UnlockCommandsIfPossible();
    }


    private void Dismantle()
    {
        cmds[1].isUsedOnce = true;
    }






}
