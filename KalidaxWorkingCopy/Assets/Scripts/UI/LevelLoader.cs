using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    private Animator animator;

    private int currentScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject.transform.parent);
        }

        animator = GetComponent<Animator>();
    }
    
    public void SwitchScene()
    {
        SceneManager.LoadScene("EndOfDayScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("EndOfDayScene");
    }

    public void SetTrigger()
    {
        animator.SetTrigger("Start");
    }
}
