using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/blackboard-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/blackboard-1b";

    private GameObject blackboardText;


    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the blackboard", noAnimation: true);
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What is the writing on the blackboard", "The writing?");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Wipe off the blackboard", "Wipe off");

        actions.Add(new ActionFlow(1, WhatIsWriting, "Sounds/blackboard-2a", "", ""));
        actions.Add(new ActionFlow(2, WipeOff, "Sounds/blackboard-3a", "Sounds/wipe-off-blackboard", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        blackboardText = GameObject.Find("Blackboard_Canvas").transform.GetChild(0).gameObject;

        base.Start();
    }




    public override void Inspect()
    {
        blackboardText.SetActive(true);
        DesksAction.UnlockCommandsIfPossible();
        base.Inspect();

    }

    private void WhatIsWriting()
    {
        cmds[1].isUsedOnce = true;
    }

    private void WipeOff()
    {
        blackboardText.SetActive(false);
        cmds[2].isUsedOnce = true;
    }






}
