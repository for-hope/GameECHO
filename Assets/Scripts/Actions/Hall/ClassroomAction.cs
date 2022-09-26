using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/classroom-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/classroom-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the classroom");
        cmds.Add(cmdAction);
        base.Start();
    }

    public override void Inspect()
    {
        base.Inspect();
        playerNavMesh.GoToTarget(GameObject.Find("Classroom_Trashbox"), () => GameManager.Instance.updateCurrentLevel(4));
    }



}