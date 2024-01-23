using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAlien : MonoBehaviour
{
    /// <summary>
    /// This is the alien that is currently roaming around in the world. We can set it to any alien
    /// we want by just switching the alien container
    /// </summary>

    //References
    [SerializeField] private SO_Alien alienContainer;
    [SerializeField] private SO_AliensInWorld aliensInWorldListSO;
    private int alienInList; //mainly for OnValidate to remove the previous one no-longer there

    //Components
    private SpriteRenderer sr;
    private Animator a;

    //Properties
    public SO_Alien m_AlienContainer { get => alienContainer; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
    }

    private void Start()
    {
        AddAlienToList();
    }

#if UNITY_EDITOR

    [ContextMenu("Update Alien")]
    private void UpdateAlien()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = alienContainer.m_AlienSprite;

        if (Application.isPlaying)
        {
            aliensInWorldListSO.worldAliens.RemoveAt(alienInList);
            AddAlienToList();
        }
    }
    
#endif

    private void AddAlienToList()
    {
        //Add this alien to the aliens in the world for the Incubation Pod UI
        aliensInWorldListSO.worldAliens.Add(alienContainer);
        alienInList = aliensInWorldListSO.worldAliens.Count - 1;


    }
}
