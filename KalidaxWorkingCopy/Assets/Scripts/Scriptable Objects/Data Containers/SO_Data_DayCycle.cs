using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Day Cycle Data Container", menuName = "Data Containers/Day Cycle")]
public class SO_Data_DayCycle : ScriptableObject
{
    public int currentDay = 0;

    //Grass Tiles if they are cut or not
    public bool[] grassTilesList;

    private void OnDisable()
    {
        grassTilesList = new bool[0];
    }

}
