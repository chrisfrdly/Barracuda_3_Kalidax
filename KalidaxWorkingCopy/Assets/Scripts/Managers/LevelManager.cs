using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadPlay(int sceneBuildIndex)
    {
        SceneManager.LoadScene("MainScenePlaytest2");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("About");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
