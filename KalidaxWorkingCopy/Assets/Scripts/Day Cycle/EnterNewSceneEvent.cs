using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNewSceneEvent : MonoBehaviour
{
    [SerializeField] private SO_AliensInWorld aliensInWorld; //on scene loaded reset List and calculate it again

  
    void Awake()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        aliensInWorld.NewSceneLoadedEventSend(SceneManager.GetActiveScene().name);
    }

}
