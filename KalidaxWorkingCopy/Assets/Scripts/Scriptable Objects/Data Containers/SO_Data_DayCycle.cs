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

    //For the Incubation Pods, Store their current state AND the amount of days they have left
    //it would be great for it to automatically sort itself in the list so we can just get the index number
    public IncubationPodData[] incubationPodData = new IncubationPodData[4];

    public bool[] incubationPodPurchased = new bool[4];

    public void Initialize()
    {
        grassTilesList = new bool[0];
        incubationPodData = new IncubationPodData[4];
        incubationPodData[0] = new IncubationPodData();
        incubationPodData[1] = new IncubationPodData();
        incubationPodData[2] = new IncubationPodData();
        incubationPodData[3] = new IncubationPodData();
        incubationPodPurchased = new bool[4];
        incubationPodPurchased[0] = true;
        currentDay = 0;
    }
    public void OnEnable()
    {
        Initialize();
    }


}
[System.Serializable]
public class IncubationPodData
{
    public int index = -1; //I set it to -1 since it should never happen and we can update the incubation pods
    public IncubationState incubationState = IncubationState.OBJ_AddSeed;
    public int daysLeft;
    public int seedIndex; //this is so that it can spawn the correct aliens
}
