using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SO_GameEvent gameEvent;
    [SerializeField] private SO_PauseMenuEventSender pauseMenuEvent;
    [SerializeField] private GameObject pauseMenuPrefab;
    [SerializeField] private SO_AliensInWorld aliensInWorld; //on scene loaded reset List and calculate it again

    //radio pager event variables

    public bool hasCollectedSeedEvent = false;
    public bool hasIncubatedAlienEvent = false;


    public static bool isGamePaused;

    private PlayerInput playerInput;
    public static GameManager Instance;
    private GameObject pauseMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        aliensInWorld.newSceneLoadedEvent.AddListener(ResetWalletAmountToAdd);

        //Calls when a player presses the pause button
        pauseMenuEvent.pauseGameEvent.AddListener(PauseTheGame);

        //Calls whenever a player presses the resume button
        pauseMenuEvent.resumeGameEvent.AddListener(ResumeTheGame);

        hasCollectedSeedEvent = false;
        hasIncubatedAlienEvent = false;

    }
    void ResetWalletAmountToAdd(string _sceneName)
    {
        if (_sceneName == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //find the playerInputHandler in the game.
        //May need to move inside function if errors when someone unpluggs controller
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
        gameEvent.RaiseProgressChanged(ProgressState.None);
    }

    private void PauseTheGame()
    {
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Menu");
   
        //Spawn in the pause menu ONLY IF IT'S THE FIRST TIME
        if (!isGamePaused)
            pauseMenu = Instantiate(pauseMenuPrefab);

        isGamePaused = true;

        //connect all the player's inputs to that pause menu's input module
        pauseMenu.GetComponent<PauseGameMenu>().ConnectControllersToPauseMenu(playerInput);

        Time.timeScale = 0;

        AudioManager.instance.LerpAudioToLevel(0.2f);
    }

    private void ResumeTheGame()
    {
        playerInput.SwitchCurrentActionMap("In-Game");

        isGamePaused = false;


        if (pauseMenu != null)
            Destroy(pauseMenu);

        Time.timeScale = 1;



        AudioManager.instance.LerpAudioToPrevLevel();

    }
}
