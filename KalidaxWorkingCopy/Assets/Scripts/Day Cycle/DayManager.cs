using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    //References
    [SerializeField] private SO_Data_DayCycle SO_Data_dayCycle;

    //Variables
    [SerializeField] private SO_GrassTileParameters SO_grassTileParams; //so we can access the respawn rate of broken grass
    private List<GrassTile> grassTiles = new List<GrassTile>(); //keep track of all grass tiles in scene so we can alter them

    //Tracked Variables for each day
    private int currentDay = 1;
    private float currentMoney = 0;

    //Property
    public int m_CurrentDay { get => currentDay; set => currentDay = value; }
    public List<GrassTile> m_GrassTiles { get => grassTiles; set => grassTiles = value; }
    public float m_CurrentMoney { get => currentMoney; set => currentMoney = value; }

    private void Awake()
    {
        //Singleton Pattern 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }


    public void NewDay()
    {
        RandomizeGrassRegrowth();
    }

    private void RandomizeGrassRegrowth()
    {
        foreach(GrassTile tile in m_GrassTiles)
        {
            //check to see which tiles are broken
            if (!tile.m_IsCut)
                continue;

            //now do the random chance for the tiles that are cut
            float chanceToRegrowSeed = SO_grassTileParams.chanceToGetSeed;

            float chancePercent = chanceToRegrowSeed / 100;

            float successThreshold = 1 - chancePercent;

            float random = Random.Range(0.0f, 1.0f);

            //if this tile won the chance to regrow, regrow
            if(random >= successThreshold)
            {
                //make the tile regrow
                tile.m_IsCut = false;
            }
        }
        Debug.Log(grassTiles.Count);
        //Send data to the Scriptable Object

        //Wipe out List
        grassTiles.Clear();

        //Load New Scene
        SceneManager.LoadScene("LucasScene");
        
    }

    //will be called from the "GrassTile.cs" script in Start to check if the cut tile is regrown for the new day
    public bool GetIsRegrown(GrassTile _grassTile)
    {
        //Get the specific element in the List from the Scriptable Object
        bool isCut = SO_Data_dayCycle.grassTilesList.Find(tile => grassTiles.Contains(_grassTile));

        //Check to see if it is a bool true or false

        return isCut;
    }
}
