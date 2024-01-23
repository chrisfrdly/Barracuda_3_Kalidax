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
}
[System.Serializable]
public class AlienInGridClickedEvent : UnityEvent<SO_Alien> { }
