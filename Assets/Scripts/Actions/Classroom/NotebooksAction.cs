using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebooksAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/notebooks-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/notebooks-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the notebooks");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Read the notebooks with your name", "Read");


        actions.Add(new ActionFlow(1, Read, "Sounds/notebooks-2a", "Sounds/read-notebooks", "notebooks-2b"));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);


        base.Start();
    }




    private void Read()
    {
        cmds[1].isUsedOnce = true;
    }

}
