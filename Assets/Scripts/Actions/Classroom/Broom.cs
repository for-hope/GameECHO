using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/broom-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/broom-1b";




    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the broom");
        cmds.Add(cmdAction);
        base.Start();
    }




    private void Search()
    {
        cmds[1].isUsedOnce = true;
    }






}
