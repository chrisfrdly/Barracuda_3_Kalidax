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


        //new action event for the intro sequence
        private event Action introScriptAction;
        public Action m_IntroScriptAction { get => introScriptAction; set => introScriptAction = value; }

        private void Awake()
        {
            Instance = this; 
        }

        public void CallIntroScriptAction()
        {
            introScriptAction?.Invoke();
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