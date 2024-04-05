using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlienButton : MonoBehaviour
{
    [SerializeField] private SO_Alien thisButtonAlien;
    [SerializeField] private SO_AliensInWorld aliensInWorldSO;
    private bool disableButton = false;
    

    public SO_Alien m_ThisButtonAlien { get => thisButtonAlien; set => thisButtonAlien = value; }
    public bool m_DisableButton { get => disableButton; set => disableButton = value; }

  

    public void Clicked()
    {
        if (disableButton) return;
        //send event to AliensInWOrld which sends event to  UIAlienGridList
        //Huge workaround because I cant have an AddListener for a button created in a for loop
        
        aliensInWorldSO.AlienInGridClickedEventSend(thisButtonAlien);
    }
}
