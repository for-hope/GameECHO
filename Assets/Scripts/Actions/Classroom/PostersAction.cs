using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostersAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/posters-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/posters-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the posters");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "What are the posters about?", "About?");
   


        actions.Add(new ActionFlow(1, PostersAbout, "Sounds/posters-2a", "", ""));
      

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
 


        base.Start();
    }




    private void PostersAbout()
    {
        cmds[1].isUsedOnce = true;
    }

 




}
