using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/brush-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/brush-1b";



    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the brush");

        cmds.Add(cmdAction);

        base.Start();
    }



}
