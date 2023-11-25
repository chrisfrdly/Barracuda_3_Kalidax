using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrassTile : MonoBehaviour
{
    //References
    [SerializeField] private SO_GrassTileParameters SO_grassTileParams;
    [SerializeField] private List<GameObject> seeds = new List<GameObject>();

    //Components
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    //Variables
    private bool isCut;
    private bool gaveSeed;

    public bool m_IsCut { get => isCut; set => isCut = value; }
    public bool m_GaveSeed { get => gaveSeed; set => gaveSeed = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();    

        //listen for the event when the player clicks the mouse button on grass
        SO_grassTileParams.breakGrassEvent.AddListener(CutTheGrass);
    }

    private void Start()
    {
        spriteRenderer.color = SO_grassTileParams.grassColours[0];

        //check to see if we're already added to the DayManager List. If not, add this tile
        bool alreadyInList = DayManager.Instance.m_GrassTiles.Contains(this);
        if (alreadyInList)
        {
            //Get Data from the scriptable Object on if it is cut or not (since at the end-
            //of the day there's a chance for it to grow back for the next day)
        }
        else
            DayManager.Instance.m_GrassTiles.Add(this);
    }

    private void CutTheGrass(Transform grassTransform)
    {
        if (isCut)
            return;

        if (grassTransform != this.transform)
            return;

        isCut = true;
        boxCollider.enabled = false;

        DropSeedChance();
    }

    private void DropSeedChance()
    {
        float chanceToDropSeed = SO_grassTileParams.chanceToGetSeed;

        float chancePercent = chanceToDropSeed / 100;

        float successThreshold = 1 - chancePercent;

        float randomNumber = Random.Range(0.00f, 1.00f);

        if(randomNumber > successThreshold)
        {
            gaveSeed = true;

            SpawnSeed();

            spriteRenderer.color = SO_grassTileParams.grassColours[1];
        }
        else
            spriteRenderer.color = SO_grassTileParams.grassColours[2];

    }

    private void SpawnSeed()
    {
        Instantiate(seeds[0], transform.position, Quaternion.identity);
    }
}
