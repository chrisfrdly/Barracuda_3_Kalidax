using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void QuitGame()
    {
        //AudioManager.instance.Play("ClickButton");
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("Positive Interact");
    }
    public void Options()
    {
        //AudioManager.instance.Play("TestSound");
    }

    public void Back()
    {
        SceneManager.LoadScene("MenuScreen");
        //AudioManager.instance.Play("ClickButton");
    }

    public void HowToPlay()
    {
        //AudioManager.instance.Play("ClickButton");
        SceneManager.LoadScene("HowToScreen");
    }


}
