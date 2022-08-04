using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoxAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/a31";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a30";
    public bool tryOpen;
    public bool useKey;
    public bool tryBreak;
    public bool useChair;
    public bool useTable;






    public new void Start()
    {
        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the metal box");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the electric box");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Use key to open the electric box");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Try to Break the electric box");
        CommandAction cmdAction5 = new CommandAction(4, TAG, "Use the chair to reach the electric box");
        CommandAction cmdAction6 = new CommandAction(5, TAG, "Use the table to reach the electric box");
        tryOpen = false;
        useKey = false;
        tryBreak = false;
        useChair = false;
        useTable = false;
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
        tryOpen = true;
    }

    public void UseKey()
    {
        Debug.Log("trying to use key to open box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a32");
        SoundManager.Instance.Play(DialogAudio);
        useKey = true;
    }

    public void TryBreak()
    {
        Debug.Log("trying to break box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a35");
        SoundManager.Instance.Play(DialogAudio);
        tryBreak = true;
    }

    public void UseChair()
    {
        Debug.Log("trying to use chair to reach the electric box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a34");
        SoundManager.Instance.Play(DialogAudio);
        useChair = true;
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
        useTable = true;
        GameObject tableObject = GameObject.Find("Table");
        //move the table to the electric box
        Vector3 newPos = new Vector3(gameObject.transform.position.x - 4, tableObject.transform.position.y, gameObject.transform.position.z);
        tableObject.transform.position = newPos;

    }

}
