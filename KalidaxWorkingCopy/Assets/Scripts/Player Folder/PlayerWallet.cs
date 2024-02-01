using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public int walletAmount; //how much money you got
    public GameObject UI_Display;
    public TextMeshProUGUI walletAmountText;
    public static PlayerWallet instance;

    private void Awake()
    {
        //Turning this script into a singleton
        if (instance == null)
        {
            instance = this;
            walletAmount = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateValue();
    }

    //this is how you put more money into the wallet
    public void Addvalue(int amount)
    {
        walletAmount += amount;
    }

    //this is how you take money out
    public void SubtractValue(int amount)
    {
        walletAmount -= amount;
    }
    
    private void UpdateValue()
    {
        walletAmountText.text = "Current Funds: " + walletAmount.ToString();
    }
}
