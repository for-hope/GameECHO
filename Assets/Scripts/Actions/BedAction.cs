using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedAction : MonoBehaviour
{
    private static string TAG = "Bed";
    // Start is called before the first fframe update
    public bool inspect;
    public bool lookUnder;
    public bool tryMove;

    private CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the bed");
    private CommandAction cmdAction2 = new CommandAction(1, TAG, "Look under the bed");
    private CommandAction cmdAction3 = new CommandAction(2, TAG, "Try to move the bed");

    private List<CommandAction> cmds = new List<CommandAction>();


    void Start()
    {
        inspect = false;
        lookUnder = false;
        tryMove = false;
  
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
        BedAction bedAction = gameObject.GetComponent<BedAction>();
        if (commandId == 0)
        {
            bedAction.Inspect();
        }
        else if (commandId == 1)
        {
            bedAction.LookUnder();
        }
        else if (commandId == 2)
        {
            bedAction.TryMove();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting The bed");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a25");
        SoundManager.Instance.Play(DialogAudio);
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        StartCoroutine(BedInspectionReply());
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;
        //find cmdAction in GameManager.commandActions
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

    public void LookUnder()
    {
        Debug.Log("looking under the bed");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a28");
        SoundManager.Instance.Play(DialogAudio);
        lookUnder = true;
    }

    public void TryMove()
    {
        Debug.Log("trying to move the bed");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a27");
        SoundManager.Instance.Play(DialogAudio);
        tryMove = true;
    }

    IEnumerator BedInspectionReply()
    { 
        yield return new WaitForSeconds(5);
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a26");
        SoundManager.Instance.Play(DialogAudio);
    }

}
