using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomWindowsAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/classroom-windows-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/classroom-windows-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the classroom windows");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What's outside the classroom windows?", "What's outside?");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the classroom windows", "Break");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Exit from the classroom windows and run away", "Run!", Visibility.HIDDEN);


        actions.Add(new ActionFlow(1, WhatsInside, "Sounds/classroom-windows-2a", "", ""));
        actions.Add(new ActionFlow(2, Break, "Sounds/classroom-windows-3a", "Sounds/break-classroom-windows", "Sounds/classroom-windows-3b"));
        actions.Add(new ActionFlow(2, Break, "Sounds/classroom-windows-4a", "", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        cmds.Add(cmdAction4);

        base.Start();
    }




    private void WhatsInside()
    {
        cmds[1].isUsedOnce = true;
    }

    private void Break()
    {
        cmds[2].isUsedOnce = true;
        GameManager.commandActions.Find(x => x.context == EnvObjects.CLASSROOM_WINDOWS.ToString() && x.id == 3).visibility = Visibility.INVISIBLE;
    }

    private void RunAway()
    {
        GameManager.Instance.Lose();
        cmds[3].isUsedOnce = true;
    }




}
