using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a17";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a40";

    //protected override string ActionEffectInspectAudioFN { get; } = "Sounds/inspect-bucket";



    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the toilet");
        CommandAction cmdAction1 = new CommandAction(1, TAG, "Break the toilet", "Break");
        CommandAction cmdAction2 = new CommandAction(2, TAG, "Take out the pipe from the toilet", "Take out Pipe*");

        cmdAction1.isUnknown = true;
        cmdAction2.isUnknown = true;
        cmds = new List<CommandAction>
        {
            cmdAction,
            cmdAction1,
            cmdAction2
        };
        actions.Add(new ActionFlow(1, TryToBreak, "", "", "Sounds/a18"));
        actions.Add(new ActionFlow(2, TakeOutPipe, "Sounds/a19", "Sounds/take-out-pipe", ""));
        base.Start();

    }


    public void TryToBreak()
    {
        cmds[1].isUsedOnce = true;
    }

    public void TakeOutPipe()
    {
        cmds[2].isUsedOnce = true;
        GameObject pipeObject = GameObject.Find("Pipe");
        if (pipeObject != null) pipeObject.SetActive(false);
        GameManager.commandActions.Find(x => x.context == EnvObjects.DOOR.ToString() && x.id == 2).visibility = Visibility.INVISIBLE;
        if (GameManager.commandActions.Find(x => x.context == EnvObjects.METAL_DOOR.ToString() && x.id == 2).isUsedOnce)
        {

            GameManager.commandActions.Find(x => x.context == EnvObjects.METAL_DOOR.ToString() && x.id == 3).visibility = Visibility.INVISIBLE;
        }


    }
}
