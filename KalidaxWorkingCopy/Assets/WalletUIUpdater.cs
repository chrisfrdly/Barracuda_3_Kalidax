using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WalletUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI walletAmountText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Attempt to find the GameObject tagged "UI"
        GameObject walletTextObj = GameObject.FindGameObjectWithTag("Wallet");

        // Check if the GameObject was found
        if (walletTextObj != null)
        {
            // Attempt to get the TextMeshProUGUI component
            walletAmountText = walletTextObj.GetComponent<TextMeshProUGUI>();

            // Check if the component was found
            if (walletAmountText != null)
            {
                // Update the wallet amount text
                UpdateWalletAmountText();
            }
            else
            {
                Debug.LogWarning("WalletUIUpdater: TextMeshProUGUI component not found on the GameObject tagged 'UI'.");
            }
        }
        else
        {
            Debug.LogWarning("WalletUIUpdater: No GameObject with tag 'UI' found in the scene.");
        }
    }



    private void OnEnable()
    {
        PlayerWallet.Instance.OnWalletAmountChanged += UpdateWalletAmountText;
    }

    private void OnDisable()
    {
        PlayerWallet.Instance.OnWalletAmountChanged -= UpdateWalletAmountText;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateWalletAmountText()
    {
        if (walletAmountText != null)
        {
            walletAmountText.text = "Wallet: $" + PlayerWallet.Instance.walletAmount.ToString();
        }
    }
}
