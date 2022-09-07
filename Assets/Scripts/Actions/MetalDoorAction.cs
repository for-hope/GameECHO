using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MetalDoorAction : VAction
{
    protected override string InitialInspectAudioFN { get; } = "Sounds/a39";
    protected override string FollowUpInspectAudioFN { get; } = "Sounds/a47";

    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the metal door");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Open the metal door", "Open");
        CommandAction cmdAction3 = new CommandAction(2, TAG, "Break the metal door", "Break");
        CommandAction cmdAction4 = new CommandAction(3, TAG, "Use the pipe to break the metal door", "Use Pipe", Visibility.HIDDEN);

        actions.Add(new ActionFlow(1, Open, "Sounds/a42", "Sounds/open-metal-door", "Sounds/a43"));
        actions.Add(new ActionFlow(2, Break, "Sounds/a44", "", ""));
        actions.Add(new ActionFlow(3, UsePipe, "Sounds/a45", "Sounds/use-pipe", "Sounds/a46", endAction: NextScene));
        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);
        cmds.Add(cmdAction3);
        cmds.Add(cmdAction4);
        base.Start();
    }


    private void Open()
    {
        return;
    }

    private void Break()
    {
        if (GameManager.commandActions.Find(x => x.context == EnvObjects.TOILET.ToString() && x.id == 2).isUsedOnce)
        {
            GameManager.commandActions.Find(x => x.context == EnvObjects.METAL_DOOR.ToString() && x.id == 3).visibility = Visibility.INVISIBLE;

        }
        return;
    }

    private void UsePipe()
    {
        // var playerPipe = GameObject.Find("PlayerPipe");
        // playerPipe.SetActive(false);
        var MetalDoorObject = GameObject.Find("Metal Door");
        MetalDoorObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        MetalDoorObject.transform.localPosition = new Vector3(-5.8f, -2.0f, MetalDoorObject.transform.position.z);


    }

    private void NextScene()
    {

        Debug.Log("NextScene");
        GameObject.Find("Fade").transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(TeleportPlayer());
        GameManager.Instance.currentLevel = 2;

    }

    IEnumerator TeleportPlayer()
    {
        yield return new WaitForSeconds(2);
        var level2StartingObj = GameObject.Find("White Door");
        playerNavMesh.GoToTarget(level2StartingObj, null);
        GameObject playerObj = GameObject.Find("PlayerCapsule");
        playerObj.SetActive(false);
        playerObj.transform.position = new Vector3(-3.26f, -0.2f, -0.71f);
        playerObj.SetActive(true);
    }

    private void onReachNextLevel()
    {
        Debug.Log("onReachNextLevel");

    }


}




