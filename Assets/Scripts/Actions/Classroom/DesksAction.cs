using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesksAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/desks-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/desks-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the desks");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Sit on your desk and fall asleep", "Sit & Sleep", Visibility.HIDDEN);



        actions.Add(new ActionFlow(1, SitAndSleep, "Sounds/desks-2a", "", ""));


        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);



        base.Start();
    }




    private void SitAndSleep()
    {
        //TODO WIN CON
        cmds[1].isUsedOnce = true;
    }







}
