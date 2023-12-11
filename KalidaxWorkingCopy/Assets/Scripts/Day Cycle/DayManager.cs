using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("GRASS TILES")]
    //References
    [SerializeField] private SO_Data_DayCycle SO_Data_dayCycle;

    //Variables
    [SerializeField] private SO_GrassTileParameters SO_grassTileParams; //so we can access the respawn rate of broken grass
    public GrassTile[] grassTiles; //keep track of all grass tiles in scene so we can alter them


    [Header("CURRENT DAY")]

    //Tracked Variables for each day
    private int currentDay;
    private float currentMoney;

    //Property
    public int m_CurrentDay { get => currentDay; set => currentDay = value; }
    public float m_CurrentMoney { get => currentMoney; set => currentMoney = value; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadGrassStates();

    }

    private void LoadGrassStates()
    {
        grassTiles = FindObjectsOfType<GrassTile>();

        //Order the grass tiles based on position in the world. Without this order, saving and loading the grass tiles
        //ends up going to a different tile instead of the one we want
        grassTiles = grassTiles.OrderBy(tile => tile.transform.position.x)
                               .ThenBy(tile => tile.transform.position.y)
                               .ToArray();

      

        //Load in the previous day's grassTile parameters into this day's grass tile's parameters
        for (int i = 0; i < SO_Data_dayCycle.grassTilesList.Length; i++)
        {
            grassTiles[i].m_IsCut = SO_Data_dayCycle.grassTilesList[i];
        }
    }

    public void NewDay()
    {
        //Increase the day counter
        SO_Data_dayCycle.currentDay++;
        currentDay = SO_Data_dayCycle.currentDay;

        //Update Grass Tiles
        SO_Data_dayCycle.grassTilesList = new bool[grassTiles.Length];

        RandomizeGrassRegrowth();


        //Saving cut data into the scriptable object
        for (int i = 0; i < SO_Data_dayCycle.grassTilesList.Length; i++)
        {
            SO_Data_dayCycle.grassTilesList[i] = grassTiles[i].m_IsCut;
        }


        SceneManager.LoadScene("EndOfDayScene");
    }


    private void RandomizeGrassRegrowth()
    {
   
        foreach (GrassTile tile in grassTiles)
        {
            //check to see which tiles are broken
            if (!tile.m_IsCut)
                continue;

            //now do the random chance for the tiles that are cut
            float chanceToRegrowSeed = SO_grassTileParams.chanceToRegrow_EndOfDay;

            float chancePercent = chanceToRegrowSeed / 100;

            float successThreshold = 1 - chancePercent;

            float random = UnityEngine.Random.Range(0.0f, 1.0f);

            //if this tile won the chance to regrow, regrow
            if(random >= successThreshold)
            {
                //make the tile regrow
                tile.m_IsCut = false;
            }
        }
    }


    public int GetCurrentDay()
    {
        return SO_Data_dayCycle.currentDay;
    }

}