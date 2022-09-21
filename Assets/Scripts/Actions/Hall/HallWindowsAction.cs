using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallWindowsAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/hall-windows-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/hall-windows-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the hall windows");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "break the hall windows with the fire extinguisher", "Break");



        actions.Add(new ActionFlow(1, Break, "Sounds/hall-windows-2a", "", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);


        base.Start();
    }




    private void Break()
    {

        cmds[1].isUsedOnce = true;

    }






}
