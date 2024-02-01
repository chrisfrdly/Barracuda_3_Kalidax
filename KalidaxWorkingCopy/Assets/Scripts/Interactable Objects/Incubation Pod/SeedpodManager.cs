using UnityEngine;

public class SeedpodManager : MonoBehaviour
{
    public GameObject seedpodPrefab; // Reference to the Seedpod prefab
    public Transform player; // Reference to the player's transform
    public float purchaseRadius = 3f; // Radius within which the player can purchase Seedpods
    public int seedpodCost = 10; // Cost of purchasing a Seedpod

    private bool isWithinRadius = false; // Flag to check if the player is within the purchase radius
    private GameObject currentSeedpod; // Reference to the current Seedpod being interacted with
    private bool canAffordSeedpod = false; // Flag to check if the player can afford the Seedpod

    private void Update()
    {
        // Check if the player is within the purchase radius
        if (isWithinRadius)
        {
            // Check if the player can afford the Seedpod
            canAffordSeedpod = CheckAffordability();

            // Handle UI feedback for Seedpod purchase
            if (canAffordSeedpod)
            {
                // Display green glow or feedback
                // Update UI to show that the Seedpod is purchasable
            }
            else
            {
                // Display red glow or feedback
                // Update UI to show that the Seedpod is not purchasable
            }

            // Handle player interaction to purchase Seedpod
            if (Input.GetKeyDown(KeyCode.E) && canAffordSeedpod)
            {
                PurchaseSeedpod();
            }
        }
        else
        {
            // Remove UI feedback for Seedpod purchase (no glow)
        }
    }

    private bool CheckAffordability()
    {
        // Implement your currency or resource management system here
        // Check if the player can afford the Seedpod (compare with seedpodCost)
        return false; // Return true if the player can afford it, else return false
    }

    private void PurchaseSeedpod()
    {
        // Deduct the cost of the Seedpod from the player's resources
        // Spawn the Seedpod in the game world at the player's position
        // Make the Seedpod interactable and functional
        // Update UI to reflect Seedpod ownership
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seedpod")) 
        {
            isWithinRadius = true;
            currentSeedpod = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Seedpod"))
        {
            isWithinRadius = false;
            currentSeedpod = null;

            // Remove UI feedback for Seedpod purchase (no glow)
        }
    }
}
