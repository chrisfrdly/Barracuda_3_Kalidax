using DS_Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SellAlienEvent : MonoBehaviour
{
    [SerializeField] SO_AliensInWorld alienInWorld;
    private WorldAlien worldAlienScript;
    private DS_InteractableObject_InteractPointConversation conversation;
    

    private void Awake()
    {
        worldAlienScript = GetComponent<WorldAlien>();
        conversation = GetComponent<DS_InteractableObject_InteractPointConversation>();
        alienInWorld.newSceneLoadedEvent.AddListener(InitializeAlienSelling);
    }
    private void Start()
    {
        InitializeAlienSelling();
    }

    private void OnDisable()
    {
        DeInitializeAlienSelling();
    }

    private void InitializeAlienSelling()
    {
        worldAlienScript = GetComponent<WorldAlien>();
        conversation = GetComponent<DS_InteractableObject_InteractPointConversation>();
        DS_GameEvents.Instance.m_SellAlienAction += SellThisAlien;
    }
    private void DeInitializeAlienSelling()
    {
        DS_GameEvents.Instance.m_SellAlienAction -= SellThisAlien;
    }

    private void SellThisAlien()
    {

        //Check to see if this alien in the DS_InteractableObject is being interacted with

        if (!conversation.m_IsTalkingTo) return;
        

        conversation.m_IsInteractable = false;
        conversation.OnPlayerExitRange();

        //set the sellAlien to true
        worldAlienScript.SetSellToTrue();
    }
}
