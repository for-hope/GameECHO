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
        base.Start();

    }


}
