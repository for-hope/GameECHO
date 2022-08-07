using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a17";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a40";

    protected override string ActionEffectInspectAudioFN { get; } = "Sounds/inspect-bucket";



    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the toilet");
        CommandAction cmdAction1 = new CommandAction(1, TAG, "Break the toilet", "Break");
        CommandAction cmdAction2 = new CommandAction(2, TAG, "Take out the pipe from the toilet", "Take out Pipe");

        cmdAction1.isUnknown = true;
        cmdAction2.isUnknown = true;
        cmds = new List<CommandAction>
        {
            cmdAction,
            cmdAction1,
            cmdAction2
        };
        actions.Add(1, TryBreak);
        actions.Add(2, TakeOutPipe);
        base.Start();

    }


    public void TryBreak()
    {
        Debug.Log("Trying to break the toilet");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a18");
        SoundManager.Instance.Play(DialogAudio);
        cmds[1].isUsedOnce = true;
    }

    public void TakeOutPipe()
    {
        Debug.Log("Taking out the pipe");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a19");
        SoundManager.Instance.Play(DialogAudio);
        cmds[2].isUsedOnce = true;
        StartCoroutine(TakeOutPipeLateAction());


    }

    IEnumerator TakeOutPipeLateAction()
    {
        yield return new WaitForSeconds(5);
        GameObject pipeObject = GameObject.Find("Pipe");
        pipeObject.SetActive(false);
        GameObject playerPipe = GameObject.Find("PlayerPipe");
        playerPipe.SetActive(true);
    }

}
