using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiresAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/wires-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/wires-1b";



    internal static void UnlockCommandsIfPossible()
    {
        bool boardTranslated = GameManager.commandActions.Find(x => x.context == EnvObjects.BOARD.ToString() && x.id == 1).isUsedOnce;
        bool mirrorShardPicked = GameManager.commandActions.Find(x => x.context == EnvObjects.MIRROR.ToString() && x.id == 2).isUsedOnce;
        bool alarmInspected = GameManager.commandActions.Find(x => x.context == EnvObjects.ALARM.ToString() && x.id == 0).isUsedOnce;
        bool unlockHiddenCommands = boardTranslated && mirrorShardPicked && alarmInspected; 
        if (unlockHiddenCommands)
        {
            GameManager.commandActions.Find(x => x.context == EnvObjects.WIRES.ToString() && x.id == 1).visibility = Visibility.INVISIBLE;
            GameManager.commandActions.Find(x => x.context == EnvObjects.WIRES.ToString() && x.id == 2).visibility = Visibility.INVISIBLE;
        }
    }



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

        GameManager.Instance.Lose();
        cmds[2].isUsedOnce = true;

    }




}
