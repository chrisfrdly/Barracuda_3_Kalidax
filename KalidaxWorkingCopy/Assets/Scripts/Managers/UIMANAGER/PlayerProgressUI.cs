using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup radioPagerCanvasGroup;
    public SO_GameEvent gameEvents;
    private Dictionary<ProgressState, string> stateMessages = new Dictionary<ProgressState, string>();
    private static HashSet<ProgressState> shownMessages = new HashSet<ProgressState>();


    private IEnumerator fadeCoroutine;

    private void Awake()
    {
        InitializeStateMessages();

    }

    // Call this method to reset progress flags when starting a new game session
    public static void ResetProgressFlags()
    {
        shownMessages.Clear();
    }

    private void InitializeStateMessages()
    {
        stateMessages[ProgressState.None] = "Proceed to the designated area on the left for seed collection. Compliance is expected.";
        stateMessages[ProgressState.SeedCollected] = "Seed acquisition confirmed. Utilize the allocated pod for seed incubation immediately. Further instructions will follow.";
        stateMessages[ProgressState.SeedPlaced] = "Seed placement acknowledged. Incubation period is two days. Sell the seeds you've collected at the provisions drone, then rest, compliance is necessary.";
        stateMessages[ProgressState.Incubating] = "Incubation in progress. Continue to gather seeds and sell them to the company. You will have more to sell soon.";
        stateMessages[ProgressState.IncubationComplete] = "Incubation phase concluded. Extract the seed from the pod for assessment. Your compliance will be noted.";
        stateMessages[ProgressState.PostIncubation] = "";
        string prevPosIncubationText = "Use the provided gene splicer to create new life. Continue to sell the life on Kalidax. \n Failure to maintain profit margins will result in termination of your position. ";
    }

    private void OnEnable()
    {
        gameEvents.onProgressChanged.AddListener(HandleProgressChange);
    }

    private void OnDisable()
    {
        gameEvents.onProgressChanged.RemoveListener(HandleProgressChange);
    }

    private void HandleProgressChange(ProgressState state)
    {
        if (!shownMessages.Contains(state) && stateMessages.ContainsKey(state))
        {
            UpdateMessage(stateMessages[state]);
            shownMessages.Add(state); // Ensure this change is reflected across scenes by making 'shownMessages' static
        }
 
        //we want to disable to radio pager
        if(state == ProgressState.PostIncubation)
        {

            fadeCoroutine = FadeRadioPager();

            if(radioPagerCanvasGroup == null)
            {
                radioPagerCanvasGroup = FindObjectOfType<CanvasGroup>();
            }
            StartCoroutine(fadeCoroutine);
            radioPagerCanvasGroup.interactable = false;
            radioPagerCanvasGroup.blocksRaycasts = false;
        }
    }

    private void UpdateMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
        else
            Debug.LogWarning("Message Text UI component is not assigned in PlayerProgressUI.");
    }

    private IEnumerator FadeRadioPager()
    {
        float a = 1;

        while(a > 0)
        {
            a -= Time.deltaTime;
            radioPagerCanvasGroup.alpha = a;
        }

        yield return null;
    }
}
