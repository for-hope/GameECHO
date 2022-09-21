using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomWindowAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/classroom-window-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/classroom-window-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the classroom window");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What's outside the classroom windows?", "What's outside?");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the classroom window", "Break");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Exit from the classroom window and run away", "Run!");



        actions.Add(new ActionFlow(1, WhatsInside, "Sounds/classroom-window-2a", "", ""));
        actions.Add(new ActionFlow(2, Break, "Sounds/classroom-window-3a", "Sounds/break-classroom-window", "Sounds/classroom-window-3b"));
        actions.Add(new ActionFlow(2, Break, "Sounds/classroom-window-4a", "", ""));

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
    }

    private void RunAway()
    {
        cmds[3].isUsedOnce = true;
    }




}
