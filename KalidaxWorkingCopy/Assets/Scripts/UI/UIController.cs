using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("End Of Day Confirmation UI")]
    [SerializeField] private GameObject endOfDayConfirmationUI;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        //Add events so when the player clicks a button, perform a function using lambda expressions
        yesButton.onClick.AddListener(() => ConfirmedDayReset());
        noButton.onClick.AddListener(() => CancelDayReset());
    }

    //This method is called from the "InteractableObject_EndOfDayMachine.cs" script
    //That function on the machine is called from the "PlayerInteractWithObject.cs" script when they press "E"
    public void ShowEndOfDayConfirmationUI()
    {
        endOfDayConfirmationUI.SetActive(true);
    }
    private void ConfirmedDayReset()
    {
        //Now the Day Manager class will handle switching to the new day!
        DayManager.Instance.NewDay();
    }
    private void CancelDayReset()
    {
        endOfDayConfirmationUI.SetActive(false);
    }
    
}
