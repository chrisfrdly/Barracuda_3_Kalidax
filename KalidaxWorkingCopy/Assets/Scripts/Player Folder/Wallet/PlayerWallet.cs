using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWallet : MonoBehaviour
{
    public int walletAmount; //how much money you got
    public static PlayerWallet instance;

    public int amountToPutInWallet; // this is the total amount of money that the player will make in a day we will use this amount in the PutValueInWallet here

    private void Awake()
    {
        //Turning this script into a singleton this will only be on the initial creation of this object
        if(instance == null)
        {
            instance = this;
            walletAmount = 0;
            amountToPutInWallet = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //this is how you put more money into the wallet
    public void PutValueInWallet(int amount)
    {
        walletAmount += amount;
        amountToPutInWallet = 0;
    }

    //this is how you take money out
    public void SubtractValue(int amount)
    {
        walletAmount -= amount;
    }


}
