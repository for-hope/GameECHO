using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalDoorAction : VAction
{
    protected override string InitialInspectAudioFN { get; } = "Sounds/a39";
    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a47";

    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the metal door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Open the metal door", "Open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the metal door", "Break");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Use the pipe to break the metal door", "Use Pipe", Visibility.HIDDEN);

        actions.Add(new ActionFlow(1, Open, "Sounds/a42", "Sounds/open-metal-door", "Sounds/a43"));
        actions.Add(new ActionFlow(2, Break, "Sounds/a44", "", ""));
        actions.Add(new ActionFlow(3, UsePipe, "Sounds/a45", "Sounds/use-pipe", "Sounds/a46"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        cmds.Add(cmdAction4);
        base.Start();
    }


    private void Open()
    {
        return;
    }

    private void Break()
    {
        return;
    }

    private void UsePipe()
    {
        // var playerPipe = GameObject.Find("PlayerPipe");
        // playerPipe.SetActive(false);
        var MetalDoorObject = GameObject.Find("Metal Door");
        MetalDoorObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        MetalDoorObject.transform.localPosition = new Vector3(-5.8f, -2.0f, MetalDoorObject.transform.position.z);


    }






}
