using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliensInWorld_Mono : MonoBehaviour
{
    public List<GameObject> aliensInWorld_GO = new List<GameObject>();
    public static AliensInWorld_Mono instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
