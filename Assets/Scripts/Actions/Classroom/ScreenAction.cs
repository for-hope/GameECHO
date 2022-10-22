using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/screen-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/screen-1b";



    private GameObject screenText;
    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the screen", noAnimation: true);
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What just played on the screen?", "What played?*");



        actions.Add(new ActionFlow(1, WhatJustPlayed, "Sounds/screen-2a", "", ""));


        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        screenText = GameObject.Find("Screen_Canvas").transform.GetChild(0).gameObject;


        base.Start();
    }


    public override void Inspect()
    {
        screenText.SetActive(true);
        DesksAction.UnlockCommandsIfPossible();

        base.Inspect();

    }

    private void WhatJustPlayed()
    {
        cmds[1].isUsedOnce = true;
    }






}
