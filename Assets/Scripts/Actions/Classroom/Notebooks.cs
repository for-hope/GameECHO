using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebooksAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/notebook-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/notebook-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the notebook");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Read the Notebooks with your name", "Read");


        actions.Add(new ActionFlow(1, Read, "Sounds/notebook-2a", "Sounds/read-notebooks", "notebooks-2b"));

        cmds.Add(cmdAction);


        base.Start();
    }




    private void Read()
    {
        cmds[1].isUsedOnce = true;
    }

}
