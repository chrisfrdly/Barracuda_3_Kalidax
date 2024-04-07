using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private SO_AliensInWorld sceneEvents; //onscene loaded we want to set the amount put in wallet to 0

    private static PlayerWallet _instance;
    public int amountToPutInWallet; // Used to accumulate daily earnings
    public int walletAmount;
    public delegate void WalletAmountChanged();
    public event WalletAmountChanged OnWalletAmountChanged;


    private int[] amountsAddedThisDay = new int[6];

    public static PlayerWallet Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerWallet>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        sceneEvents.newSceneLoadedEvent.AddListener(ResetWalletAmountToAdd);
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PlayerWallet] Instance assigned and set to DontDestroyOnLoad.", this);
        }
        else if (_instance != this)
        {
            Debug.Log("[PlayerWallet] Duplicate instance detected, destroying this one.", this);
            Destroy(gameObject);
        }

        
    }

    void ResetWalletAmountToAdd(string _sceneName)
    {
        amountToPutInWallet = 0;
        Debug.Log("NEW SCENE LOADED");

        if (_sceneName == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        if(_sceneName == "EndOfDayScene")
        {
            //Then we will replace the gold filler text with the amounts they gained and lost

            //Find the EndOfDay Amount
            EndOfDayAmounts eod = FindObjectOfType<EndOfDayAmounts>();
            eod.m_AmountsToShow = amountsAddedThisDay;
            
            foreach(int a in amountsAddedThisDay)
            {
                Debug.Log(a);
            }
           
        }
      
        amountsAddedThisDay = new int[6];

        
    }

    // Call this method to add earnings throughout the day
    public void AddToAmountToPutInWallet(int amount, string reason)
    {
        if(reason == "Sold Item")
        {
            amountsAddedThisDay[0] += amount;
        }
        else if(reason == "Sold Alien")
        {
            amountsAddedThisDay[1] += amount;
        }
        
    }

    public void TransferToWallet(string reason)
    {
        walletAmount += amountToPutInWallet;
        amountToPutInWallet = 0;
        Debug.Log($"Transferring {amountToPutInWallet} to wallet due to: {reason}. New Total: {walletAmount}");
        OnWalletAmountChanged?.Invoke();
    }

    public void SubtractValue(int amount, string reason)
    {
        walletAmount -= amount;
        OnWalletAmountChanged?.Invoke();

        if (reason == "End of Day Quota")
        {
            amountsAddedThisDay[2] += amount;
        }
        else if (reason == "Incubation Pod Purchased")
        {
            amountsAddedThisDay[3] += amount;
        }
        else if (reason == "Removed Wall")
        {
            amountsAddedThisDay[3] += amount;
        }
    }
}
