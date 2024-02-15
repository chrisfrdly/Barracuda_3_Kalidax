using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGameMenu : MonoBehaviour
{
    /// <summary>
    /// The Pause Menu Prefab is instantiated into the scene from the 'PlayerInputHandler' when a player
    /// clicks the pause button.
    /// The 'GameManager' is in charge of checking to see if the game is paused or not.
    /// </summary>

    //components
    [SerializeField] private SO_PauseMenuEventSender pauseMenuEvent;

    [SerializeField] private Button resumeButton;


    [SerializeField] private RectTransform controlsPanelRect;
    private float minPosY;
    private float maxPosY;

    private void Awake()
    {
        //hover over the 'resume button' on startup
        if (PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
            resumeButton.Select();

    }

    public void ConnectControllersToPauseMenu(PlayerInput player)
    {
        AudioManager.instance.Play("Positive Interact");

        //Get all the playuer's inputUI Modules and connect them to this UI input module in child
        player.uiInputModule = GetComponentInChildren<InputSystemUIInputModule>();
  
    }

    public void ResumeGameButton()
    {
        //AudioManager.instance.Play("ClickButton");

        AudioManager.instance.Play("Negative Interact");


        //Send event to the game manager to resume the game
        pauseMenuEvent.ResumeGameEventSend();

        //remove this pause menu
        Destroy(transform.root.gameObject);

    }

    public void PauseMenu_TitleScreen()
    {
        //AudioManager.instance.Play("ClickButton");

        AudioManager.instance.Play("Negative Interact");

        //Send event to the game manager to resume the game
        pauseMenuEvent.ResumeGameEventSend();

        //Load Scene
        //SceneManager.LoadScene("");


    }

    // CONTROLS SCREEN IN THE PAUSE MENU //

    //Called from the button in the inspector
    public void ControlsButton()
    {
        //Leen Tween from top of frame to bottom
        LeanTween.value(1080f, 0, 0.5f).setOnUpdate(UpdatePanelMinY).setEaseOutQuad().setIgnoreTimeScale(true);
        LeanTween.value(1080f, 0, 0.5f).setOnUpdate(UpdatePanelMaxY).setEaseOutQuad().setIgnoreTimeScale(true);

        AudioManager.instance.Play("Positive Interact");

    }

    //Called from the button in the inspector
    public void ControlsBackButton()
    {
        //Lean tween from the bottom to the top
        LeanTween.value(0, 1080, 0.5f).setOnUpdate(UpdatePanelMinY).setEaseInQuad().setIgnoreTimeScale(true);
        LeanTween.value(0, 1080, 0.5f).setOnUpdate(UpdatePanelMaxY).setEaseInQuad().setIgnoreTimeScale(true);

        AudioManager.instance.Play("Negative Interact");

    }

    private void UpdatePanelMinY(float _value)
    {
        minPosY = _value;
        controlsPanelRect.offsetMin = new Vector2(controlsPanelRect.offsetMin.x, minPosY);
    }
    private void UpdatePanelMaxY(float _value)
    {
        maxPosY = _value;
        controlsPanelRect.offsetMax = new Vector2(controlsPanelRect.offsetMax.x, maxPosY);
    }
}



