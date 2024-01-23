using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlienButton : MonoBehaviour
{
    [SerializeField] private SO_Alien thisButtonAlien;

    public SO_Alien m_ThisButtonAlien { get => thisButtonAlien; set => thisButtonAlien = value; }

    [SerializeField] private SO_AliensInWorld aliensInWorldSO;

    public void Clicked()
    {
        //send event to AliensInWOrld which sends event to  UIAlienGridList
        //Huge workaround because I cant have an AddListener for a button created in a for loop
        
        aliensInWorldSO.AlienInGridClickedEventSend(thisButtonAlien);
    }
}
