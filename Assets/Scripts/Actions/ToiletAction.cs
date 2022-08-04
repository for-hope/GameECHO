using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletAction : VAction
{

    public bool tryBreak;
    public bool takeOutPipe;

    protected override string InitialInspectAudioFN { get; } = "Sounds/a17";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a40";


     public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the toilet");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Break the toilet");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Take out the pipe from the toilet");
        tryBreak = false;
        takeOutPipe = false;
        cmdAction2.isUnknown = true;
        cmdAction3.isUnknown = true;
        cmds = new List<CommandAction>
        {
            cmdAction,
            cmdAction2,
            cmdAction3
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
        tryBreak = true;
    }

    public void TakeOutPipe()
    {
        Debug.Log("Taking out the pipe");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a19");
        SoundManager.Instance.Play(DialogAudio);
        takeOutPipe = true;
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
