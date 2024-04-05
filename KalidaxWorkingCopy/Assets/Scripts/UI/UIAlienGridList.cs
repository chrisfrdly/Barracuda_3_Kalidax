
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class UIAlienGridList : MonoBehaviour
{
    //Variables
    private Transform gridParent;


    //Get a reference to a manager that contains all the Aliens
    [SerializeField] private SO_AliensInWorld aliensInWorldSO;//this will have an event sent here
    [SerializeField] private GameObject alienButtonPrefab;

    //we need to store all the buttons created in the grid in a list so we can delete them onEnable every time
    private List<GameObject> buttonList = new List<GameObject>();

    private InteractableObject_GeneSplicingPod io;

    //Properties


    private void Awake()
    {
        gridParent = this.transform;

        //Sent from the UIAlienButton.cs
        aliensInWorldSO.alienInGridClickedEvent.AddListener(HideAlienGridPanel);
        
        //Get the incubation pod root
        io = transform.GetComponentInParent<InteractableObject_GeneSplicingPod>();

    }

    private void OnEnable()
    {


        //delete all previous buttons
        if(buttonList.Count !=0)
        {
            for (int i = buttonList.Count-1; i >= 0; i--)
            {
                Destroy(buttonList[i]);
            }
            
        }
        buttonList.Clear();

        for (int i = 0; i < aliensInWorldSO.worldAliens.Count; i++)
        {
           
            
            //Spawn button object
            buttonList.Add(Instantiate(alienButtonPrefab, gridParent));


            //Select the first alien in the list for controllers
            if (i == 0 && PlayerInputHandler.Instance.GetCurrentControlScheme() == "Controller")
                buttonList[i].GetComponent<Button>().Select();

            //Get the button image and set it to the Alien's profile image
            Image buttonImg = buttonList[i].GetComponentInChildren<Image>();
            buttonImg.sprite = aliensInWorldSO.worldAliens[i].m_AlienSprite;

   
            //Get the button text and set it to the alien's tier 
            TextMeshProUGUI txt = buttonList[i].GetComponentInChildren<TextMeshProUGUI>();
            txt.text = aliensInWorldSO.worldAliens[i].m_AlienTier.ToString();

            UIAlienButton alienButton = buttonList[i].GetComponent<UIAlienButton>();
            alienButton.m_ThisButtonAlien = aliensInWorldSO.worldAliens[i];



            //Check to see if this alien is selected already. If so, then colour it darker
            //Check to see the Game Object
            SO_Alien worldAlien = aliensInWorldSO.worldAliens[i].m_ThisAlien;
            alienButton.m_DisableButton = false;

            if (i > 1) continue;
            if (io.m_AliensAdded[i] == worldAlien && io.m_AddAlienColour[i] == true)
            {
                buttonImg.color = new Color(0.4f, 0.4f, 0.4f);
                alienButton.m_DisableButton = true;
            }





        }
    }


    public void HideAlienGridPanel(SO_Alien _alien)
    {
        //Set the logic. Basically we'll set the button pressed 
       
        io.SetAlien(_alien);
        gameObject.SetActive(false);
        
        //When we click on the button, return the alien
    }
}
