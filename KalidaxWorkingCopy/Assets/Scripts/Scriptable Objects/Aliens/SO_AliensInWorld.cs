using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName ="Aliens in World", menuName = "Aliens/Aliens In World")]
public class SO_AliensInWorld : ScriptableObject
{
    public List<SO_Alien> worldAliens = new List<SO_Alien>();

    //Recieved from the UIAlienButton and sent to the UIAlienGridList
    [System.NonSerialized]
    public AlienInGridClickedEvent alienInGridClickedEvent = new AlienInGridClickedEvent();

    public void AlienInGridClickedEventSend(SO_Alien _alien)
    {
        alienInGridClickedEvent.Invoke(_alien);
    }

    //Called from the DayCyle 
    //Recieved from the Aliens In World that are under the DoNotDestroyOnLoad();
    [System.NonSerialized]
    public UnityEvent newSceneLoadedEvent = new UnityEvent();

    public void NewSceneLoadedEventSend()
    {
        newSceneLoadedEvent.Invoke();
    }

    //Called from the DayCyle 
    //Recieved from the Aliens In World that are under the DoNotDestroyOnLoad();
    [System.NonSerialized]
    public UnityEvent sceneExittedEvent = new UnityEvent();

    public void SceneExittedEventSend()
    {
        sceneExittedEvent.Invoke();
    }
}
[System.Serializable]
public class AlienInGridClickedEvent : UnityEvent<SO_Alien> { }
