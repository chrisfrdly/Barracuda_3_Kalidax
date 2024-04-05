using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using Unity.VisualScripting;

public class DS_DialogueController : MonoBehaviour
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


    private void Awake()
    {
        ShowDialogueUI(false);

       
    }

    public void ShowDialogueUI(bool _show)
    {
        dialogueUI.SetActive(_show);
        UIController.Instance.m_CurrentUIVisible = dialogueUI;
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

        if (_image == null)
            return;

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

        
    }

    private void DeleteButtons()
    {
        for(int i = buttonParentPanel.childCount - 1; i >= 0 ; i--)
        {
            Destroy(buttonParentPanel.GetChild(i).gameObject);
        }
    }
}
