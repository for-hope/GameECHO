using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/sink-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/sink-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the Sink");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Destroy the sink", "Destroy");

        actions.Add(new ActionFlow(1, DestroySink, "Sounds/sink-2a", "", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);


        base.Start();
    }


    public void DestroySink()
    {
        cmds[1].isUsedOnce = true;
    }



}
