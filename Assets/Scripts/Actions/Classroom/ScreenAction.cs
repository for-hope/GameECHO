using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/screen-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/screen-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the screen");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What just played on the screen?", "What played?");



        actions.Add(new ActionFlow(1, WhatJustPlayed, "Sounds/screen-2a", "", ""));


        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);


        base.Start();
    }




    private void WhatJustPlayed()
    {
        cmds[1].isUsedOnce = true;
    }






}
