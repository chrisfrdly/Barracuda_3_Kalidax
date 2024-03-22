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
    public static bool isGameInitialized = false;

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

        if (!isGameInitialized)
        {
            //persistence (oops I made a dependancy)
            DontDestroyOnLoad(gameObject);

            // Initialize the game for the first time
            InitializeGame();
            isGameInitialized = true; // Set the flag to true to avoid re-initialization
        }

        //Calls when a player presses the pause button
        pauseMenuEvent.pauseGameEvent.AddListener(PauseTheGame);

        //Calls whenever a player presses the resume button
        pauseMenuEvent.resumeGameEvent.AddListener(ResumeTheGame);

        //Set Audio

    }

    private void Start()
    {
        //find the playerInputHandler in the game.
        //May need to move inside function if errors when someone unpluggs controller
        playerInput = GameObject.FindObjectOfType<PlayerInput>();
    }

    private void InitializeGame()
    {
        gameEvent.RaiseProgressChanged(ProgressState.None); // Signal the game start event
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
