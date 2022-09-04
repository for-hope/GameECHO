using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/mirror-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/mirror-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the mirror");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Break the mirror", "Break");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Pick a shard from the broken mirror", "Pick Shard", Visibility.HIDDEN);


        actions.Add(new ActionFlow(1, CutWiresOrangeDoor, "Sounds/mirror-2a", "Sounds/break-mirror", "Sounds/mirror-2b"));
        actions.Add(new ActionFlow(2, CutWiresWhiteDoor, "Sounds/mirror-3a", "Sounds/pick-mirror-shard", "Sounds/mirror-3b"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void BreakMirror()
    {
        //TODO: Cut the wires near the orange door
        cmds[1].isUsedOnce = true;

    }

    public void PickShards()
    {

        //TODO: Cut the wires near the white door and LOSE
        cmds[2].isUsedOnce = true;

    }




}
