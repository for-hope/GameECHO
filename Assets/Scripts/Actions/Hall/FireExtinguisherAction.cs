using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/fire-extinguisher-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/fire-extinguisher-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the fire extinguisher");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Plug the fire extinguisher to the fireplug", "Plug");



        actions.Add(new ActionFlow(1, Plug, "Sounds/fire-extinguisher", "", ""));


        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);

        base.Start();
    }

    public override void Inspect()
    {
        //find command hallwindows
        GameManager.commandActions.Find(x => x.context.ToLower() == EnvObjects.HALL_WINDOWS.ToString().ToLower() && x.id == 1).visibility = Visibility.INVISIBLE;
        base.Inspect();
    }

    private void Plug()
    {

        cmds[1].isUsedOnce = true;

    }






}
