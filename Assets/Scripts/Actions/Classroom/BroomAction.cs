using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/broom-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/broom-1b";




    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the broom");
        cmds.Add(cmdAction);
        base.Start();
    }

    public override void Inspect()
    {
        base.Inspect();
        GameManager.commandActions.Find(x => x.context == EnvObjects.CLASSROOM_DOOR.ToString() && x.id == 1).visibility = Visibility.INVISIBLE;

    }



    private void Search()
    {
        cmds[1].isUsedOnce = true;
    }






}
