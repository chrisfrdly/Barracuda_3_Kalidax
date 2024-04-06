using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRadioPager : MonoBehaviour
{
    public bool radioPagerEnable = true;
    [SerializeField] private float timer = 5f;
    private PlayerProgressUI progressUI;

    private void Awake()
    {
       progressUI = FindObjectOfType<PlayerProgressUI>();
        //just checking for the current state 
        if(progressUI.gameEvents.currentState == ProgressState.PostIncubation)
        {
            radioPagerEnable = false;
        }

    }
    // Update is called once per frame
    void Update()
    {

        if (!radioPagerEnable)
        {
            Invoke("TurnButtonOff", timer);
        }
    }
    
    public void TurnButtonOff()
    {
        gameObject.SetActive(false);
    }
}
