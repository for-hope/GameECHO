using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/aXX";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/aXX";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the alarm");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Dismantle the alarm", "Dismantle");



        actions.Add(new ActionFlow(1, Dismantle, "Sounds/alarm-2a", "", ""));

        cmds.Add(cmdAction);

    
        base.Start();
    }



    public void Dismantle()
    {
        
        cmds[1].isUsedOnce = true;

    }

 




}
