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
    [SerializeField] private AliensInWorld_Mono objectListScript;
    public GameObject provisionDroneObject;
    private int alienInList; //mainly for OnValidate to remove the previous one no-longer there

    //the variables here are going to help get our alien to the provisions drone and sell it once in range
    [SerializeField] private float moveSpeed; // this is gonna be so that the alien can move towards the provisions drone
    private float distanceToDrone = 0.1f;
    public bool isBeingSold;
    //Components
    private SpriteRenderer sr;
    private Animator a;

    //Properties
    public SO_Alien m_AlienContainer { get => alienContainer; }

    private void Awake()
    {
        provisionDroneObject = FindObjectOfType<InteractableObject_Provisions_Drone>().gameObject;
        objectListScript = FindObjectOfType<AliensInWorld_Mono>();
        isBeingSold = false;
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
        aliensInWorldListSO.newSceneLoadedEvent.AddListener(InitializeAlien);
    }

    private void Start()
    {
        objectListScript.aliensInWorld_GO.Add(gameObject);
        AddAlienToList();
        UpdateAlienInGame();
    }
    private void InitializeAlien()
    {
        provisionDroneObject = FindObjectOfType<InteractableObject_Provisions_Drone>().gameObject;
        objectListScript = FindObjectOfType<AliensInWorld_Mono>();
    }
    private void Update()
    {
        if (isBeingSold)
        {
            MoveToProvisionsDrone();
        }
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

    private void UpdateAlienInGame()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = alienContainer.m_AlienSprite;

        aliensInWorldListSO.worldAliens.RemoveAt(alienInList);
        AddAlienToList();
       
    }

    private void AddAlienToList()
    {
        //Add this alien to the aliens in the world for the Incubation Pod UI
        aliensInWorldListSO.worldAliens.Add(alienContainer);
        alienInList = aliensInWorldListSO.worldAliens.Count - 1;
        
        
    }

    //this method gets the allien close to the drone and sells it after
    private void MoveToProvisionsDrone()
    {
        transform.position = Vector3.MoveTowards(transform.position, provisionDroneObject.transform.position, moveSpeed * Time.deltaTime);

        if(Vector3.Distance(provisionDroneObject.transform.position, transform.position) < distanceToDrone)
        {
            SellAlien();
        }
    }

    private void SellAlien()
    {
        PlayerWallet.Instance.amountToPutInWallet += alienContainer.sellValue;
        Debug.Log("Is Being Sold");
        DestroyAlien();
    }

    //this is just so we can call this method on a button press we'll do this when we make the aliens interactable
    public void SetSellToTrue()
    {
        isBeingSold = true;
    }

    //this destroys the aliens at any point, can be called from other scripts. This is made so that we can destroy the alien without necessarily selling it
    public void DestroyAlien()
    {
        aliensInWorldListSO.worldAliens.Remove(alienContainer);
        objectListScript.aliensInWorld_GO.Remove(gameObject);
        Destroy(gameObject);
    }

}
