using DS_Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellAlienEvent : MonoBehaviour
{
    private WorldAlien worldAlienScript;
    private DS_InteractableObject_InteractPointConversation conversation;

    private void Awake()
    {
        worldAlienScript = GetComponent<WorldAlien>();
        conversation = GetComponent<DS_InteractableObject_InteractPointConversation>();
    }
    private void Start()
    {
        DS_GameEvents.Instance.m_SellAlienAction += SellThisAlien;
    }

    private void OnDisable()
    {
        DS_GameEvents.Instance.m_SellAlienAction -= SellThisAlien;
    }

    private void SellThisAlien()
    {
        //Check to see if this alien in the DS_InteractableObject is being interacted with

        if (!conversation.InPlayerRange) return;

      
        conversation.m_IsInteractable = false;
        conversation.OnPlayerExitRange();

        //set the sellAlien to true
        worldAlienScript.SetSellToTrue();
    }
}
