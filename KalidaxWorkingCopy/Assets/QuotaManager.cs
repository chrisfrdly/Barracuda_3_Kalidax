using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance;

    [Header("References")]
    [SerializeField] private SO_Data_DayCycle dayCycleData;
    [SerializeField] private SO_Data_CurrentQuota currentQuotaData;
    [SerializeField] private TextMeshProUGUI quotaDisplayText;

    private int lastCheckedDay = -1; // Track the last day a quota check was performed

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to find the GameObject that contains your quota display text
        GameObject quotaTextObj = GameObject.FindGameObjectWithTag("QuotaDisplay");

        // Check if the GameObject was found
        if (quotaTextObj != null)
        {
            // Get the TextMeshProUGUI component
            quotaDisplayText = quotaTextObj.GetComponent<TextMeshProUGUI>();

            // Update the quota display text
            if (quotaDisplayText != null)
            {
                UpdateQuotaDisplay();
            }
            else
            {
                Debug.LogWarning("QuotaManager: TextMeshProUGUI component for quota display not found.");
            }
        }
        else
        {
            Debug.LogWarning("QuotaManager: No GameObject with the 'QuotaDisplay' tag found.");
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (dayCycleData.currentDay != lastCheckedDay)
        {
            UpdateQuotaDisplay();
            lastCheckedDay = dayCycleData.currentDay;
        }
    }

    public int GetQuotaForDay(int day)
    {
        int currentDayIndex = day - 1; // Adjust index to start from 0
        if (currentDayIndex >= 0 && currentDayIndex < currentQuotaData.quotaArray.Length)
        {
            return currentQuotaData.quotaArray[currentDayIndex];
        }
        return 0; // Default to no quota if out of bounds
    }


    private void UpdateQuotaDisplay()
    {
        int quotaForToday = GetQuotaForDay(dayCycleData.currentDay);
        if (quotaForToday > 0)
        {
            quotaDisplayText.text = $"Day {dayCycleData.currentDay} Quota: {quotaForToday}";
        }
        else
        {
            quotaDisplayText.text = "No Quota Today";
            DisplayNextQuotaCountdown();
        }
    }


    private void DisplayNextQuotaCountdown()
    {
        int currentDayIndex = dayCycleData.currentDay - 1;  // Adjust index to start from 0
        int daysUntilNextQuota = 0;
        bool nextQuotaFound = false;

        for (int i = currentDayIndex + 1; i < currentQuotaData.quotaArray.Length; i++)
        {
            daysUntilNextQuota++;
            if (currentQuotaData.quotaArray[i] > 0)
            {
                nextQuotaFound = true;
                break;
            }
        }

        if (nextQuotaFound)
        {
            int nextQuota = currentQuotaData.quotaArray[currentDayIndex + daysUntilNextQuota];
            quotaDisplayText.text += $"\nNext Quota: {nextQuota} in {daysUntilNextQuota} days";
        }
        else
        {
            quotaDisplayText.text += "\nNo upcoming quotas.";
        }
    }

}
