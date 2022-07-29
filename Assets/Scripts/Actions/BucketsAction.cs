using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketsAction : MonoBehaviour
{
    public static string TAG = "bucket";
    // Start is called before the first frame update
    public bool inspect;
    public bool move;
    public bool search;
    private CommandAction cmdAction = new CommandAction(0,TAG, "Inspect the buckets");
    private CommandAction cmdAction2 = new CommandAction(1,TAG, "Move the buckets");
    private CommandAction cmdAction3 = new CommandAction(2,TAG, "Search the buckets");

    private List<CommandAction> cmds = new List<CommandAction>();


    void Start()
    {
        inspect = false;
        move = false;
        search = false;
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
        GameObject gameObject = GameObject.Find("Buckets");
        BucketsAction bucketsAction = gameObject.GetComponent<BucketsAction>();
        if (commandId == 0)
        {
            bucketsAction.Inspect();
        }
        else if (commandId == 1)
        {
            bucketsAction.Move();
        }
        else if (commandId == 2)
        {
            bucketsAction.Search();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting Bucket");
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a7");
        SoundManager.Instance.Play(DialogAudio);
        StartCoroutine(InspectResponse());
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;
        for (int i = 0; i < GameManager.commandActions.Count; i++)
        {
            CommandAction gmCmdAction = GameManager.commandActions[i];
            for (int j = 0; j < cmds.Count; j++)
            {
                if (gmCmdAction.context == cmds[j].context && cmds[j].id == 0)
                {
                    GameManager.commandActions[i].isUsedOnce = true;
                }
                else if (gmCmdAction.context == cmds[j].context)
                {
                    GameManager.commandActions[i].isJustLearnt = true;
                    GameManager.commandActions[i].isUnknown = false;
                    
                }

            }
        }
    }

    public void Move()
    {
        Debug.Log("Moving Bucket");
        //TODO voice answer
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 5, gameObject.transform.position.y, gameObject.transform.position.z);
        move = true;
    }

    public void Search()
    {
        Debug.Log("Searching Bucket");
        search = true;
        //TODO voice answer
    }
    
    IEnumerator InspectResponse()
    {

        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a8");
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(6);
        SoundManager.Instance.Play(DialogAudio);


    }

}
