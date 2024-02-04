using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WalletDisplay : MonoBehaviour
{
    public TextMeshProUGUI walletAmountText;

    private void Update()
    {
        UpdateDisplayValue();
    }
    private void UpdateDisplayValue()
    {
        walletAmountText.text = PlayerWallet.instance.walletAmount.ToString();
    }
}
