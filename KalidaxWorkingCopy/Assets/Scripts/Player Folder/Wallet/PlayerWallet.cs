using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private SO_AliensInWorld sceneEvents; //onscene loaded we want to set the amount put in wallet to 0

    private static PlayerWallet _instance;
    public int amountToPutInWallet; // Used to accumulate daily earnings
    public int walletAmount;
    public delegate void WalletAmountChanged();
    public event WalletAmountChanged OnWalletAmountChanged;

    public static PlayerWallet Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerWallet>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        sceneEvents.newSceneLoadedEvent.AddListener(ResetWalletAmountToAdd);
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[PlayerWallet] Instance assigned and set to DontDestroyOnLoad.", this);
        }
        else if (_instance != this)
        {
            Debug.Log("[PlayerWallet] Duplicate instance detected, destroying this one.", this);
            Destroy(gameObject);
        }

        
    }

    void ResetWalletAmountToAdd(string _sceneName)
    {
        amountToPutInWallet = 0;

        if (_sceneName == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        
    }

    // Call this method to add earnings throughout the day
    public void AddToAmountToPutInWallet(int amount)
    {
        amountToPutInWallet += amount;
    }

    public void TransferToWallet(string reason)
    {
        walletAmount += amountToPutInWallet;
        amountToPutInWallet = 0;
        Debug.Log($"Transferring {amountToPutInWallet} to wallet due to: {reason}. New Total: {walletAmount}");
        OnWalletAmountChanged?.Invoke();
    }

    public void PutValueInWallet(int amount, string reason)
    {
        walletAmount += amount;
        
        Debug.Log($"Adding {amount} to wallet due to: {reason}. New Total: {walletAmount}");
        OnWalletAmountChanged?.Invoke();
    }

    public void SubtractValue(int amount, string reason)
    {
        walletAmount -= amount;
        
        Debug.Log($"Subtracting {amount} from wallet due to: {reason}. New Total: {walletAmount}");
        OnWalletAmountChanged?.Invoke();
    }
}
