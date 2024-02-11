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
        isBeingSold = false;
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();
    }

    private void Start()
    {
        AddAlienToList();
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

    private void AddAlienToList()
    {
        //Add this alien to the aliens in the world for the Incubation Pod UI
        aliensInWorldListSO.worldAliens.Add(alienContainer);
        alienInList = aliensInWorldListSO.worldAliens.Count - 1;


    }

    //this method gets the allien close to the drone and sells it after
    private void MoveToProvisionsDrone()
    {
        transform.position = Vector3.MoveTowards(transform.position, provisionDroneObject.transform.position, moveSpeed);

        if(Vector3.Distance(provisionDroneObject.transform.position, transform.position) < distanceToDrone)
        {
            SellAlien();
        }
    }

    private void SellAlien()
    {
        PlayerWallet.instance.amountToPutInWallet += alienContainer.sellValue;
        aliensInWorldListSO.worldAliens.Remove(alienContainer);
        Destroy(gameObject);
    }

    //this is just so we can call this method on a button press we'll do this when we make the aliens interactable
    public void SetSellToTrue()
    {
        isBeingSold = true;
    }
}
