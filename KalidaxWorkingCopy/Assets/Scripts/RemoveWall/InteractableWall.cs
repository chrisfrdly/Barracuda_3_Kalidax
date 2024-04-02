using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractableWall : InteractableObject
{

    private PlayerWallet wallet;
    [Space(4)]
    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;
    [Space(4)]
    [Header("Cost")]
    public int RemoveWallCost;

    private void Start()
    {
        GameObject wallObj = GameObject.Find("Wallet Manager");
        wallet = wallObj.GetComponent<PlayerWallet>();

        GameObject wallSprite = GameObject.Find("Wall Sprite");
        spriteRenderer = wallSprite.GetComponent<SpriteRenderer>();
    }
    public override void OnInteract(GameObject _interactedActor)
        {
            if(wallet.walletAmount >= RemoveWallCost)
            {
                wallet.SubtractValue(RemoveWallCost, "Removed Wall");
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(CannotRemove());
            }
        }

    IEnumerator CannotRemove()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

        public override bool CheckIsInteractable() { return isInteractable; }
        public override bool IsTargetPointVisible() { return isInteractPointVisible; }
        public override bool FreezePlayerMovement() { return freezePlayerMovement; }
        public override bool IsRequiredToLookAtTarget() { return isRequiredToLookAtTarget; }


    }

