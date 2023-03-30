using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{

    public void LoadGame()
    {
        SceneManager.LoadScene("EconomyTestingSceneBotsOP2");

    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame");
    }
}
