using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAction : MonoBehaviour
{
    private static string TAG = "Door";
    // Start is called before the first fframe update
    public bool inspect;
    public bool tryOpen;
    public bool tryBreak;
    private CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the door");
    private CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the door");
    private CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the door with the pipe");

    private List<CommandAction> cmds = new List<CommandAction>();



    void Start()
    {
        inspect = false;
        tryOpen = false;
        tryBreak = false;
        cmds = new List<CommandAction>
        {
            cmdAction,
            cmdAction2,
            cmdAction3
        };
        GameManager.commandActions.AddRange(cmds);
    }

    public static void triggerAction(int commandId)
    {
        GameObject gameObject = GameObject.Find(TAG);
        DoorAction doorAction = gameObject.GetComponent<DoorAction>();
        if (commandId == 0)
        {
            doorAction.Inspect();
        }
        else if (commandId == 1)
        {
            doorAction.TryOpen();
        }
        else if (commandId == 2)
        {
            doorAction.TryBreak();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting The door");
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a9");
        SoundManager.Instance.Play(DialogAudio);
        StartCoroutine(InspectResponse());
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;
        for (int i = 0; i < GameManager.commandActions.Count; i++)
        {
            CommandAction gmCmdAction = GameManager.commandActions[i];
            for (int j = 0; j < cmds.Count; j++)
            {
                if (gmCmdAction != cmds[j])
                {
                    GameManager.commandActions[i].isJustLearnt = false;
                    continue;
                }
                if (cmds[j].id == 0)
                {
                    GameManager.commandActions[i].isUsedOnce = true;
                }
                else
                {
                    GameManager.commandActions[i].isJustLearnt = !GameManager.commandActions[i].isUnknown;
                    GameManager.commandActions[i].isUnknown = false;
                }

            }
        }

    }

    public void TryOpen()
    {
        Debug.Log("Trying to open the door");
        tryOpen = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a11");
        SoundManager.Instance.Play(DialogAudio);
    }

    public void TryBreak()
    {
        Debug.Log("Trying to break the door");
        tryBreak = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a12");
        SoundManager.Instance.Play(DialogAudio);
        //TODO voice answer
    }

    IEnumerator InspectResponse()
    {

        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a10");
        AudioClip InspectActionAudio = Resources.Load<AudioClip>("Sounds/door-inspect");
        
        yield return new WaitForSeconds(5);
        SoundManager.Instance.Play(InspectActionAudio);

        yield return new WaitForSeconds(5);
        SoundManager.Instance.Play(DialogAudio);


    }
}
