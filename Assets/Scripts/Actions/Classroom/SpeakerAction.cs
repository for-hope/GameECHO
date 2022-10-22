using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/speaker-1a";
    protected override string FollowUpInspectAudioFN { get; } = "Sounds/speaker-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the speaker", noAnimation: true);
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Did the speakers just say something?", "Speaker says?*");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Remind me what the speakers said", "Remind*");


        actions.Add(new ActionFlow(1, SpeakerSays, "Sounds/speaker-2a", "", ""));
        actions.Add(new ActionFlow(2, Remind, "Sounds/speaker-3a", "", ""));

        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);


        base.Start();
    }


    public override void Inspect()
    {
        DesksAction.UnlockCommandsIfPossible();

        base.Inspect();
    }

    private void SpeakerSays()
    {
        cmds[1].isUsedOnce = true;
    }

    private void Remind()
    {
        cmds[2].isUsedOnce = true;
    }






}
