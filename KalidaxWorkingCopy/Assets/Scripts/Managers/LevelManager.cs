using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private SO_Data_DayCycle SO_Data_dayCycle;

    private void Awake()
    {
        CheckIfMainMenuLoaded();
    }

    private void CheckIfMainMenuLoaded()
    {
        //If we go back to the main scene then reset the data of the seeds
        //This script should only be in the main menu, so it should work
        if (SceneManager.GetActiveScene().name != "MainMenu") return;
       
        SO_Data_dayCycle.Initialize();
        PlayerProgressUI.ResetProgressFlags();

    }

    public void LoadPlay(int sceneBuildIndex)
    {
        SceneManager.LoadScene("IntroScene");
        AudioManager.instance.Play("Positive Interact");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("Negative Interact");
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

    public void LoadExit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        AudioManager.instance.Play("Negative Interact");
    }
}
