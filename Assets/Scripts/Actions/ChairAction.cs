using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairAction : VAction
{

    protected override string LateReplyAudioFileName { get;} = "Sounds/a23";
    



    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the chair");
        cmds.Add(cmdAction);
        base.Start();
    }



}
