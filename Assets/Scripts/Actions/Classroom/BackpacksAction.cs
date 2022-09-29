using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpacksAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/backpacks-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/backpacks-1b";




    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the backpacks");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Search the backpacks", "Search");
        actions.Add(new ActionFlow(1, Search, "Sounds/backpacks-2a", "Sounds/search-backpack", "Sounds/backpacks-2b"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        base.Start();
    }




    private void Search()
    {
        cmds[1].isUsedOnce = true;
    }






}
