using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoxAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a31";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a30";





    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the metal box");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the electric box", "Try to open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Use key to open the electric box", "Use key");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Try to Break the electric box", "Try to Break");
        CommandAction cmdAction5 = new CommandAction(4, TAG, "Use the chair to reach the electric box", "Use Chair", Visibility.HIDDEN);
        CommandAction cmdAction6 = new CommandAction(5, TAG, "Use the table to reach the electric box", "Use Table", Visibility.HIDDEN);

        cmds = new List<CommandAction>
        {
            cmdAction,
            cmdAction2,
            cmdAction3,
            cmdAction4,
            cmdAction5,
            cmdAction6
        };
        actions.Add(1, TryOpen);
        actions.Add(2, UseKey);
        actions.Add(3, TryBreak);
        actions.Add(4, UseChair);
        actions.Add(5, UseTable);
        base.Start();

    }





    public void TryOpen()
    {
        Debug.Log("trying to open electric box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a28");
        SoundManager.Instance.Play(DialogAudio);
        cmds[1].isUsedOnce = true;
    }

    public void UseKey()
    {
        Debug.Log("trying to use key to open box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a32");
        SoundManager.Instance.Play(DialogAudio);
        cmds[2].isUsedOnce = true;
    }

    public void TryBreak()
    {
        Debug.Log("trying to break box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a35");
        SoundManager.Instance.Play(DialogAudio);
        cmds[3].isUsedOnce = true;
    }

    public void UseChair()
    {
        Debug.Log("trying to use chair to reach the electric box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a34");
        SoundManager.Instance.Play(DialogAudio);
        cmds[4].isUsedOnce = true;
        cmds[5].isUsedOnce = true;
        GameObject chairObject = GameObject.Find("Chair");
        //move the chair to the electric box
        Vector3 newPos = new Vector3(gameObject.transform.position.x - 1, chairObject.transform.position.y, gameObject.transform.position.z);
        chairObject.transform.position = newPos;
    }

    public void UseTable()
    {
        Debug.Log("trying to use table to reach the electric box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a33");
        SoundManager.Instance.Play(DialogAudio);
        cmds[4].isUsedOnce = true;
        cmds[5].isUsedOnce = true;
        GameObject tableObject = GameObject.Find("Table");
        //move the table to the electric box
        Vector3 newPos = new Vector3(gameObject.transform.position.x - 4, tableObject.transform.position.y, gameObject.transform.position.z);
        tableObject.transform.position = newPos;

    }

}
