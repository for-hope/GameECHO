using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAction : VAction
{

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a24";

    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the table");
        cmds.Add(cmdAction);
        actions.Add(new ActionFlow(0, InspectTable, FollowUpInspectAudioFN, "", ""));

        base.Start();

    }

    private void InspectTable()
    {
        GameManager.commandActions.Find(x => x.context == EnvObjects.METAL_BOX.ToString() && x.id == 5).visibility = Visibility.INVISIBLE;	
    }
}
