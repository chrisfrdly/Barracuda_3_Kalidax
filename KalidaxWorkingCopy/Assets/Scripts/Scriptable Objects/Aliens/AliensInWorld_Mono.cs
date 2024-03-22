using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensInWorld_Mono : MonoBehaviour
{
    [SerializeField] private SO_AliensInWorld aliensInWorld; //on scene loaded reset List and calculate it again

    public List<GameObject> aliensInWorld_GO = new List<GameObject>();
    public static AliensInWorld_Mono instance;

    private void Awake()
    {

       

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        aliensInWorld.newSceneLoadedEvent.AddListener(GetChildrenInList);

    }

    private void OnEnable()
    {
        aliensInWorld.worldAliens.Clear();
    }
    //Add all the children to the aliensInWorld List
    private void GetChildrenInList()
    {
        //Need to clear the List every awake or else will stack with every play.
        //Cannot clear in the "UIAlienGrisList" since it's awake is called after
        //the aliens are added to the list, so they will be removed
        aliensInWorld.worldAliens.Clear();
        aliensInWorld_GO.Clear();
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject alienChild = transform.GetChild(i).gameObject;
            aliensInWorld_GO.Add(alienChild);

            SO_Alien alienContainer = alienChild.GetComponent<WorldAlien>().m_AlienContainer;
            aliensInWorld.worldAliens.Add(alienContainer);
        }
    }

    public void SetAllItemsInactive()
    {
        foreach(GameObject aliens in aliensInWorld_GO)
        {
            aliens.gameObject.SetActive(false);
        }

    }

    public void SetAllItemsActive()
    {
        foreach(GameObject aliens in aliensInWorld_GO)
        {
            aliens.gameObject.SetActive(true);
        }
    }
}
