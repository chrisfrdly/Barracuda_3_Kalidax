using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    //References
    [SerializeField] private SO_GrassTileParameters grassTileParams;
    [SerializeField] private List<GameObject> seeds = new List<GameObject>();

    //Components
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    //Variables
    private bool isCut = false;
    private bool gaveSeed;

    public bool m_IsCut { get => isCut; set => isCut = value; }
    public bool m_GaveSeed { get => gaveSeed; set => gaveSeed = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();    

        //listen for the event when the player clicks the mouse button on grass
        grassTileParams.breakGrassEvent.AddListener(CutTheGrass);
    }

    private void Start()
    {
        spriteRenderer.color = grassTileParams.grassColours[0];
    }

    private void CutTheGrass(Transform grassTransform)
    {
        if (isCut)
            return;

        if (grassTransform != this.transform)
            return;

        Debug.Log("The Grass is Broken");

        isCut = true;
        boxCollider.enabled = false;

        DropSeedChance();
    }

    private void DropSeedChance()
    {
        float chanceToDropSeed = grassTileParams.chanceToGetSeed;

        float chancePercent = chanceToDropSeed / 100;

        float successThreshold = 1 - chancePercent;

        float randomNumber = Random.Range(0.00f, 1.00f);

        if(randomNumber > successThreshold)
        {
            gaveSeed = true;

            SpawnSeed();

            spriteRenderer.color = grassTileParams.grassColours[1];

            Debug.Log("Dropped Seed");
        }
        else
        {
            spriteRenderer.color = grassTileParams.grassColours[2];

            Debug.Log("Didn't Drop Seed");
        }
    }

    private void SpawnSeed()
    {
        Instantiate(seeds[0], transform.position, Quaternion.identity);
    }
}
