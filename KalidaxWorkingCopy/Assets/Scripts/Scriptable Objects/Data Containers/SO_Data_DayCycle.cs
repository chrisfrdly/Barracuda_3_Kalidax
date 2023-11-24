using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day Cycle Data Container", menuName = "Data Containers/Day Cycle")]
public class SO_Data_DayCycle : ScriptableObject
{
    //Grass Tiles if they are cut or not
    public List<GrassTile> grassTilesList = new List<GrassTile>();
}
