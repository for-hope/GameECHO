using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairAction : VAction
{

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a23";




    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the chair");
        cmds.Add(cmdAction);
        actions.Add(new ActionFlow(0, InspectChair, FollowUpInspectAudioFN, "", ""));

        base.Start();
    }

    private void InspectChair()
    {
        GameManager.commandActions.Find(x => x.context == EnvObjects.METAL_BOX.ToString() && x.id == 4).visibility = Visibility.INVISIBLE;
    }

}
