using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/locker-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/locker-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the locker");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Open the lockers", "Open");



        actions.Add(new ActionFlow(1, OpenLocker, "Sounds/locker-2a", "Sounds/open-locker", "Sounds/locker-2b"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        base.Start();
    }



    public void OpenLocker()
    {
        cmds[1].isUsedOnce = true;

    }

}
