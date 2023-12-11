using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day Cycle Data Container", menuName = "Data Containers/Day Cycle")]
public class SO_Data_DayCycle : ScriptableObject
{
    //Grass Tiles if they are cut or not
    public bool[] grassTilesList;

    private void OnDisable()
    {
        //reset the tile list when exit the editor (FOR NOW)
        grassTilesList = new bool[0];
    }


}
