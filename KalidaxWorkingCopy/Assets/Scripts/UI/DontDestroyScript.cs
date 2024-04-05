using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    [SerializeField] private SO_AliensInWorld aliensInWorld;

    private void Awake()
    {
        aliensInWorld.newSceneLoadedEvent.AddListener(DestroyOnMainMenuLoaded);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);        
    }

    void DestroyOnMainMenuLoaded(string _sceneName)
    {
        if(_sceneName == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }
        
    }

}
