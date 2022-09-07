using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/board-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/board-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the board");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Translate the writing in the Board", "Translate");


        actions.Add(new ActionFlow(1, Translate, "", "", "Sounds/board-2a"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        base.Start();
    }



    public void Translate()
    {
        cmds[1].isUsedOnce = true;
        WiresAction.UnlockCommandsIfPossible();
    }





}
