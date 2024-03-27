using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentDayUIText : MonoBehaviour
{

    void Start()
    {
        //This is for updating the current day text in the UI
        TextMeshProUGUI currentDayText = GetComponent<TextMeshProUGUI>();
        if (currentDayText != null)
        {
            currentDayText.text = $"Day: {DayManager.Instance.GetCurrentDay() + 1}";
        }

    }

}
