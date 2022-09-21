using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/blackboard-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/blackboard-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the blackboard");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What is the writing on the blackboard", "The writing?");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Wipe off the blackboard", "Wipe off");


        actions.Add(new ActionFlow(1, WhatIsWriting, "Sounds/blackboard-2a", "", ""));
        actions.Add(new ActionFlow(2, WipeOff, "Sounds/blackboard-3a", "Sounds/wipe-off-blackboard", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);


        base.Start();
    }




    private void WhatIsWriting()
    {
        cmds[1].isUsedOnce = true;
    }

    private void WipeOff()
    {
        cmds[2].isUsedOnce = true;
    }






}
