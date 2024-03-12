using UnityEngine;

public class DS_LanguageController : MonoBehaviour
{
    [SerializeField] private DS_LanguageType languageType;

    //Singleton
    public static DS_LanguageController Instance { get; private set; }
    public DS_LanguageType LanguageType { get => languageType; set => languageType = value; }

    private void Awake()
    {
        //Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
            
        else
            Destroy(this.gameObject);
    }
}
