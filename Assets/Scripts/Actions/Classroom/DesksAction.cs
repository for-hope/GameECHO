using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesksAction : VAction
{

    protected override string InitialInspectAudioFN { get; } = "Sounds/desks-1a";

    protected override string FollowUpInspectAudioFN { get; } = "Sounds/desks-1b";




    public new void Start()
    {

        CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the desks");
        CommandAction cmdAction2 = new CommandAction(1, TAG, "Sit on your desk and fall asleep", "Sit & Sleep", Visibility.HIDDEN);



        actions.Add(new ActionFlow(1, SitAndSleep, "Sounds/desks-2a", "", "", endAction: endScene));


        cmds.Add(cmdAction);
        cmds.Add(cmdAction2);



        base.Start();
    }


    internal static void UnlockCommandsIfPossible()
    {
        bool blackboardInspected = GameManager.commandActions.Find(x => x.context == EnvObjects.BLACKBOARD.ToString() && x.id == 0).isUsedOnce;
        bool screenInspected = GameManager.commandActions.Find(x => x.context == EnvObjects.SCREEN.ToString() && x.id == 0).isUsedOnce;
        bool speakerInspected = GameManager.commandActions.Find(x => x.context == EnvObjects.SPEAKER.ToString() && x.id == 0).isUsedOnce;
        bool notebookRead = GameManager.commandActions.Find(x => x.context == EnvObjects.NOTEBOOKS.ToString() && x.id == 1).isUsedOnce;
        bool[] rules = { blackboardInspected, screenInspected, speakerInspected, notebookRead };

        int validRules = 0;
        for (int i = 0; i < rules.Length; i++)
        {
            if (rules[i])
            {
                validRules++;
            }
        }
        bool unlockHiddenCommands = validRules >= 4;
        if (unlockHiddenCommands)
        {
            GameManager.commandActions.Find(x => x.context == EnvObjects.DESKS.ToString() && x.id == 1).visibility = Visibility.INVISIBLE;
        }
    }



    private void SitAndSleep()
    {
        //IntroManager.Instance.ShowEndingConfirmation(IntroManager.Ending.ESCAPE_BY_SLEEPING);
        
        Debug.Log("SIT AND SLEEP");
        GameObject.Find("Fade").transform.GetChild(0).gameObject.SetActive(true);
        cmds[1].isUsedOnce = true;
    }

    private void endScene()
    {
        Debug.Log("END SCENE");
        StartCoroutine(EndingScene());
    }


    IEnumerator EndingScene()
    {
        SoundManager.Instance.Play(Resources.Load<AudioClip>("Sounds/ending-waking-up"));

        yield return new WaitForSeconds(6f);
        //WIN
        //enable characters
        GameObject.Find("Characters").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("Characters").transform.GetChild(1).gameObject.SetActive(true);
        //move player to //
        GameObject player = GameObject.Find("PlayerCapsule");
        //animate isEnding to true
        SoundManager.Instance.PlayMusic(Resources.Load<AudioClip>("Sounds/ending_background_school"));
        SoundManager.Instance.Play(Resources.Load<AudioClip>("Sounds/ending-dialog"));
        //get cinemachine and change priority to 11
        GameObject.Find("EndingCam").GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority = 11;
        playerNavMesh.GoToTarget(GameObject.Find("Player_Chair"), null);
        player.GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = false;
        player.GetComponent<StarterAssets.FirstPersonController>().enabled = false;
        player.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        player.GetComponent<PlayerNavMesh>().enabled = false;
        player.SetActive(false);
        player.transform.position = new Vector3(-9.25f, -0.4f, -0.55f);
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
        player.SetActive(true);
        GameObject.Find("Character").GetComponent<Animator>().SetBool("isEnding", true);
        yield return new WaitForSeconds(15f);
        SceneManager.LoadSceneAsync("EndScene");

    }




}
