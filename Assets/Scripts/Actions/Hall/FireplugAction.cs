using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireplugAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/fireplug-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/fireplug-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the fireplug");
        cmds.Add(cmdAction);

        base.Start();
    }


}
