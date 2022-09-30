using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/mirror-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/mirror-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the mirror");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Break the mirror", "Break");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Pick a shard from the broken mirror", "Pick Shard", Visibility.HIDDEN);


        actions.Add(new ActionFlow(1, BreakMirror, "Sounds/mirror-2a", "Sounds/break-mirror", "Sounds/mirror-2b"));
        actions.Add(new ActionFlow(2, PickShards, "Sounds/mirror-3a", "Sounds/pick-mirror-shard", "Sounds/mirror-3b"));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        base.Start();
    }



    public void BreakMirror()
    {
        GameObject.Find("Right Mirror").GetComponent<MeshDestroy>().DestroyMesh();
        cmds[1].isUsedOnce = true;
        GameManager.commandActions.Find(x => x.context == EnvObjects.MIRROR.ToString() && x.id == 2).visibility = Visibility.INVISIBLE;
    }

    public void PickShards()
    {
        cmds[2].isUsedOnce = true;
        WiresAction.UnlockCommandsIfPossible();

    }




}
