using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoxAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a29";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a30";





    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the metal box");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the metal box", "Try to open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Use a key to open the metal box", "Use a key*");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Try to Break the metal box", "Try to Break");
        CommandAction cmdAction5 = new CommandAction(4, TAG, "Use the chair to reach the metal box", "Use Chair*", Visibility.HIDDEN);
        CommandAction cmdAction6 = new CommandAction(5, TAG, "Use the table to reach the metal box", "Use Table*", Visibility.HIDDEN);

        cmds = new List<CommandAction>
        {
            cmdAction,
            cmdAction2,
            cmdAction3,
            cmdAction4,
            cmdAction5,
            cmdAction6
        };
        var accessCmds = new List<int>
        {
           4,
           5
        };
        actions.Add(new ActionFlow(1, TryToOpen, "", "Sounds/open-metal_box", "Sounds/a31", accessCmds, noAccessAudioFilename: "Sounds/metal-box-cant-reach"));
        actions.Add(new ActionFlow(2, UseKey, "", "", "Sounds/a32"));
        actions.Add(new ActionFlow(3, TryToBreak, "Sounds/a35", "", "", accessCmds, "Sounds/metal-box-cant-reach"));
        actions.Add(new ActionFlow(4, UseChair, "Sounds/a34", "Sounds/moving-chair", ""));
        actions.Add(new ActionFlow(5, UseTable, "Sounds/a34", "Sounds/moving-table", ""));
        base.Start();

    }





    public void TryToOpen()
    {
       
        cmds[1].isUsedOnce = true;
    }

    public void UseKey()
    {
        cmds[2].isUsedOnce = true;
    }

    public void TryToBreak()
    {
        GameManager.Instance.Lose();
        cmds[3].isUsedOnce = true;
    }

    public void UseChair()
    {
        cmds[4].isUsedOnce = true;
        cmds[5].isUsedOnce = true;
        GameObject chairObject = GameObject.Find("Chair");
        //move the chair to the electric box
        Vector3 newPos = new Vector3(-2.0f, -2.0f, 7.0f);
        chairObject.transform.localPosition = newPos;
    }

    public void UseTable()
    {
        cmds[4].isUsedOnce = true;
        cmds[5].isUsedOnce = true;
        GameObject tableObject = GameObject.Find("Table");
        //move the table to the electric box
        Vector3 newPos = new Vector3(-2.8f, -2.0f, 6.8f);
        tableObject.transform.localPosition = newPos;

    }

}
