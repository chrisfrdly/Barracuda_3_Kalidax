using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndOfDayAmounts : MonoBehaviour
{
    private int[] amountsToShow = new int[6];
    private string[] reason = new string[6];
    public int[] m_AmountsToShow { get => amountsToShow; set => amountsToShow = value; }

    //0:  + Sold Item
    //1:  + Sold Alien
    //2:  - Quota
    //3:  - Incubation Pods Purchased
    //4:  - Wall Cut

    //5: Total Left

    //Now Get a reference to the Text of all the things
    [SerializeField] private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
    [SerializeField] private List<TextMeshProUGUI> reasonsTextList = new List<TextMeshProUGUI>();

    private void Start()
    {
        reason[0] = "Sold Items";
        reason[1] = "Sold Aliens";
        reason[2] = "Quota";
        reason[3] = "Incubation Pods Purchased";
        reason[4] = "Wall Removed";
        reason[5] = "Total";

        //On start we want to update the text with all the int amounts
        if (PlayerWallet.Instance == null) return;

        for(int i = 0; i <textList.Count; i++)
        {
            reasonsTextList[i].text = reason[i];

            if (i == textList.Count -1)
            {
                textList[i].text = $"${PlayerWallet.Instance.walletAmount}.00";
                break;
            }

            string sign = i <= 1 ? "+ " : "- ";
            
            textList[i].text = $"{sign} ${amountsToShow[i]}.00";
        }
    }

}
