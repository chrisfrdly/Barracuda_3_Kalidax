using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GrassTile : MonoBehaviour
{
    //References
    [SerializeField] private SO_GrassTileParameters SO_grassTileParams;

    [SerializeField] private List<GameObject> seeds = new List<GameObject>();

    //Components
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    //Variables
    [SerializeField] private bool isCut;
    private bool droppedSeed;

    public bool m_IsCut { get => isCut; set => isCut = value; }
    public bool m_DroppedSeed { get => droppedSeed; set => droppedSeed = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();    

        //listen for the event when the player clicks the mouse button on grass
        SO_grassTileParams.breakGrassEvent.AddListener(CutTheGrass);
    }
    private void Start()
    {
        //change colour of grass tile depending on state
        Invoke("UpdateTile",0.05f);
    }

    private void CutTheGrass(Transform _grassTransform)
    {
        if (isCut)
            return;

        if (_grassTransform != this.transform)
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

        float randomNumber = UnityEngine.Random.Range(0.00f, 1.00f);

        if (randomNumber > successThreshold)
        {
            droppedSeed = true;

            SpawnSeed();
        }

        UpdateTile();
    }

    private void SpawnSeed()
    {
        Instantiate(seeds[0], transform.position, Quaternion.identity);
    }

    private void UpdateTile()
    {
        if(!isCut)
        {
            spriteRenderer.color = SO_grassTileParams.grassColours[0];
            boxCollider.enabled = true;
        }
        else
        {
            spriteRenderer.color = SO_grassTileParams.grassColours[1];
            boxCollider.enabled = false;
            
        }
    }

}
