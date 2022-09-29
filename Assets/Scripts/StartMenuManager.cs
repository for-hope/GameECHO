using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private GameObject instructionsWindow;
    void Start()
    {
        instructionsWindow = this.transform.Find("Instructions").gameObject;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void StartGame()
    {
        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowInstructions()
    {
        instructionsWindow.SetActive(true);
    }
    public void HideInstructions()
    {
        instructionsWindow.SetActive(false);
    }

}
