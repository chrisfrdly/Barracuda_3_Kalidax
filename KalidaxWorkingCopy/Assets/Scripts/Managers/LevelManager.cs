using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadPlay(int sceneBuildIndex)
    {
        SceneManager.LoadScene("MainScene_Playtest3");
        AudioManager.instance.Play("Positive Interact");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("Positive Interact");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("About");
        AudioManager.instance.Play("Positive Interact");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
        AudioManager.instance.Play("Positive Interact");
    }
}
