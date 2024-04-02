using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using Unity.VisualScripting;

public class DS_DialogueControllerAsh : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private RectTransform textBoxParent;
    [SerializeField] private TextMeshProUGUI dialogueTextBox;

    [Header("Images")]
    [SerializeField] private Image leftImage; //the image we want to change
    [SerializeField] private GameObject leftImageGO; //hiding and unhiding image. We're using a mask so we need 2 different variables
    [SerializeField] private Image rightImage;
    [SerializeField] private GameObject rightImageGO;

    [Header("Buttons")]
    [SerializeField] private RectTransform buttonParentPanel;
    [SerializeField] private Button buttonPrefab;

    private VerticalLayoutGroup layoutGroup;
    private float spacing;
    private float topPadding;

    private Vector2 buttonParentPanelSize;
    private Vector2 buttonPrefabSize;

    private void Awake()
    {
        ShowDialogueUI(false);

        //Setting up button parameters
        layoutGroup = buttonParentPanel.GetComponent<VerticalLayoutGroup>();

        spacing = layoutGroup.spacing;
        topPadding = layoutGroup.padding.top;

        buttonParentPanelSize = buttonParentPanel.sizeDelta;
        buttonPrefabSize = buttonPrefab.GetComponent<RectTransform>().sizeDelta;
    }

    public void ShowDialogueUI(bool _show)
    {
        dialogueUI.SetActive(_show);
    }

    //change the name and the text in the UI
    public void SetText(string _textName, string _textBox)
    {
        
        textName.text = _textName;
        dialogueTextBox.text = _textBox;

    }


    //change the image in the UI
    public void SetImage(Sprite _image, DS_DialogueSpriteImageType _dialogueSpriteImageType)
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
    public void SetButtons(List<string> _buttonTexts, List<UnityAction> _unityActions)
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

        buttonParentPanel.sizeDelta = new Vector2(buttonParentPanelSize.x, 
                                                  buttonPrefabSize.y * _buttonTexts.Count + spacing * _buttonTexts.Count + (topPadding + spacing));
    }

    private void DeleteButtons()
    {
        for(int i = buttonParentPanel.childCount - 1; i >= 0 ; i--)
        {
            Destroy(buttonParentPanel.GetChild(i).gameObject);
        }
    }
}
