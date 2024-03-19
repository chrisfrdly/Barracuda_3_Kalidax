using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS_Events
{


    public class DS_GameEvents : MonoBehaviour
    {
       //We don't want to keep this event when changing scene. 

        //we can put a lot of methods inside of this action
        private event Action sellAlienAction;

        public static DS_GameEvents Instance { get; private set; }

        public Action m_SellAlienAction { get => sellAlienAction; set => sellAlienAction = value;}

        private void Awake()
        {
            Instance = this; 
        }

        public void CallSellAlienAction()
        {
            //When invoking, it will run all the methods inside this action

            //The ? means it will only invoke if there's something inside me
            //the same as::
            //if(sellAlienAction != null)
            sellAlienAction?.Invoke();
        }

    }
}