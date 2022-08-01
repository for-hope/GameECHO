using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrashAction : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inspect;
    public bool move;
    public bool search;
    private CommandAction cmdAction = new CommandAction(0, "trash", "Inspect the trash");
    private CommandAction cmdAction2 = new CommandAction(1, "trash", "Move the trash");
    private CommandAction cmdAction3 = new CommandAction(2, "trash", "Search inside the trash");
    private List<CommandAction> cmds = new List<CommandAction>();
    private PlayerNavMesh playerNavMesh;


    void Start()
    {
        playerNavMesh = GameObject.Find("PlayerCapsule").GetComponent<PlayerNavMesh>();
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
        GameObject gameObject = GameObject.Find("Trash");
        TrashAction trashAction = gameObject.GetComponent<TrashAction>();
        if (commandId == 0)
        {
            PlayerNavMesh.OnReachedTargetDelegate onReachedTarget = new PlayerNavMesh.OnReachedTargetDelegate(trashAction.Inspect);
            trashAction.playerNavMesh.GoToTarget(gameObject, onReachedTarget);
        }
        else if (commandId == 1)
        {
            trashAction.Move();
        }
        else if (commandId == 2)
        {
            trashAction.Search();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting Trash");
        gameObject.GetComponent<VoiceObject>().isInspected = true;
        inspect = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a13");
        SoundManager.Instance.Play(DialogAudio);
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

    public void Move()
    {
        Debug.Log("Moving trash");
        //TODO voice answer
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a16");
        SoundManager.Instance.Play(DialogAudio);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 5, gameObject.transform.position.y, gameObject.transform.position.z);
        move = true;

    }

    public void Search()
    {
        Debug.Log("Searching inside trash");
        search = true;
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a15");
        SoundManager.Instance.Play(DialogAudio);
    }

    IEnumerator LateInspectReply()
    {
        yield return new WaitForSeconds(8);
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a14");
        SoundManager.Instance.Play(DialogAudio);
    }

}
