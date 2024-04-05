using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using Unity.VisualScripting;

public class DS_DialogueControllerAsh : DS_DialogueController // Inherit from DS_DialogueController
{
    private HorizontalLayoutGroup layoutGroup;
    private float spacing;
    private float topPadding;

    private Vector2 buttonParentPanelSize;
    private Vector2 buttonPrefabSize;

    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method

        // Setting up additional parameters specific to DS_DialogueControllerAsh
        layoutGroup = buttonParentPanel.GetComponent<HorizontalLayoutGroup>();
        spacing = layoutGroup.spacing;
        topPadding = layoutGroup.padding.top;
        buttonParentPanelSize = buttonParentPanel.sizeDelta;
        buttonPrefabSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
    }

    public override void ShowDialogueUI(bool _show)
    {
        dialogueUI.SetActive(_show);
    }

    //change the name and the text in the UI
    public override void SetText(string _textName, string _textBox)
    {
        
        textName.text = _textName;
        dialogueTextBox.text = _textBox;

    }


    //change the image in the UI
    public override void SetImage(Sprite _image, DS_DialogueSpriteImageType _dialogueSpriteImageType)
    {
        leftImageGO.SetActive(false);
        rightImageGO.SetActive(false);

        if (_image == null)
            return;

        if(_dialogueSpriteImageType == DS_DialogueSpriteImageType.Left)
        {
            leftImage.sprite = _image;
            leftImageGO.SetActive(true);

            //if it's an image on the right we want the dialogue text box to be anchored on the right
            textBoxParent.pivot = new Vector2(1, 0);
            textBoxParent.anchorMin = new Vector2(1, 0);
            textBoxParent.anchorMax = new Vector2(1, 0);

            
        }
        else
        {
            rightImage.sprite = _image;
            rightImageGO.SetActive(true);

            //if it's an image on the right we want the dialogue text box to be anchored on the left
            textBoxParent.pivot = new Vector2(0, 0);
            textBoxParent.anchorMin = new Vector2(0, 0);
            textBoxParent.anchorMax = new Vector2(0, 0);
        }
        textBoxParent.anchoredPosition = Vector2.zero;
    }


    //Unity Actions are an action. Add a new action to the button so when I click the button it does the action
    //Actions are the OnClick() events but we're doing it through code.
    public override void SetButtons(List<string> _buttonTexts, List<UnityAction> _unityActions)
    {

        DeleteButtons();

        for (int i = 0; i < _buttonTexts.Count; i++)
        {
            Button button = Instantiate(buttonPrefab, buttonParentPanel);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            //set the button's text
            buttonText.text = _buttonTexts[i];

            button.onClick.AddListener(_unityActions[i]);  

        }

        
    }

    private void DeleteButtons()
    {
        for(int i = buttonParentPanel.childCount - 1; i >= 0 ; i--)
        {
            Destroy(buttonParentPanel.GetChild(i).gameObject);
        }
    }
}
