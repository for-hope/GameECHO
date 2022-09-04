using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/wires-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/wires-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the wires");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Cut the wires near the orange door", "Cut wires on orange door", Visibility.HIDDEN);
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Cut the wires near the white door", "Cut wires on white door", Visibility.HIDDEN);


        actions.Add(new ActionFlow(1, CutWiresOrangeDoor, "Sounds/wires-2a", "Sounds/cut-wires", "Sounds/wires-2b"));
        actions.Add(new ActionFlow(2, CutWiresWhiteDoor, "Sounds/wires-3a", "Sounds/cut-wires", "Sounds/wires-3b"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void CutWiresOrangeDoor()
    {
        //TODO: Cut the wires near the orange door
        cmds[1].isUsedOnce = true;

    }

    public void CutWiresWhiteDoor()
    {

        //TODO: Cut the wires near the white door and LOSE
        cmds[2].isUsedOnce = true;

    }




}
