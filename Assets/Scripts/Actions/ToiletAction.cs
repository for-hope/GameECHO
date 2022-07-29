using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletAction : MonoBehaviour
{
    private static string TAG = "Toilet";
    // Start is called before the first fframe update
    public bool inspect;
    public bool tryBreak;
    public bool takeOutPipe;
    private CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the toilet");
    private CommandAction cmdAction2 = new CommandAction(1, TAG, "Break the toilet");
    private CommandAction cmdAction3 = new CommandAction(2, TAG, "Take out the pipe from the toilet");
    private List<CommandAction> cmds = new List<CommandAction>();



    void Start()
    {
        inspect = false;
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
        GameManager.commandActions.AddRange(cmds);
    }

    // Update is called once per frame

    public static void triggerAction(int commandId)
    {
        GameObject gameObject = GameObject.Find(TAG);
        ToiletAction toiletAction = gameObject.GetComponent<ToiletAction>();
        if (commandId == 0)
        {
            toiletAction.Inspect();
        }
        else if (commandId == 1)
        {
            toiletAction.TryBreak();
        }
        else if (commandId == 2)
        {
            toiletAction.TakeOutPipe();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting The toilet");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a17");
        SoundManager.Instance.Play(DialogAudio);
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        GameObject pipeObject = GameObject.Find("Pipe");
        pipeObject.layer = 6;

        //TODO voice answer
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
