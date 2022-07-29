using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBoxAction : MonoBehaviour
{
    private static string TAG = "Metal Box";
    // Start is called before the first fframe update
    public bool inspect;
    public bool tryOpen;
    public bool useKey;
    public bool tryBreak;
    public bool useChair;
    public bool useTable;
    private CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the metal box");
    private CommandAction cmdAction2 = new CommandAction(1, TAG, "Try to open the electric box");
    private CommandAction cmdAction3 = new CommandAction(2, TAG, "Use key to open the electric box");
    private CommandAction cmdAction4 = new CommandAction(3, TAG, "Try to Break the electric box");
    private CommandAction cmdAction5 = new CommandAction(4, TAG, "Use the chair to reach the electric box");
    private CommandAction cmdAction6 = new CommandAction(5, TAG, "Use the table to reach the electric box");
    private List<CommandAction> cmds = new List<CommandAction>();




    void Start()
    {
        inspect = false;
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
        GameManager.commandActions.AddRange(cmds);
    }

    public static void triggerAction(int commandId)
    {
        GameObject gameObject = GameObject.Find(TAG);
        MetalBoxAction metalBoxAction = gameObject.GetComponent<MetalBoxAction>();
        if (commandId == 0)
        {
            metalBoxAction.Inspect();
        }
        else if (commandId == 1)
        {
            metalBoxAction.TryOpen();
        }
        else if (commandId == 2)
        {
            metalBoxAction.UseKey();
        } else if (commandId == 3)
        {
            metalBoxAction.TryBreak();
        } else if (commandId == 4)
        {
            metalBoxAction.UseChair();
        } else if (commandId == 5)
        {
            metalBoxAction.UseTable();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting The metal box");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a31");
        SoundManager.Instance.Play(DialogAudio);
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        StartCoroutine(MetalBoxInspectionReply());
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
        Vector3 newPos = new Vector3(gameObject.transform.position.x-1, chairObject.transform.position.y, gameObject.transform.position.z);
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

    IEnumerator MetalBoxInspectionReply()
    {
        yield return new WaitForSeconds(8);
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a30");
        SoundManager.Instance.Play(DialogAudio);
    }

}
