
using UnityEngine;


public class SO_Alien : ScriptableObject
{
    private GameObject alienGO;
    private SO_AliensContainer _container;
    private SO_Alien _thisAlien;

    //[Title("Alien Information",TextAlignment.Center)]
    [SerializeField] private string _name;
    [SerializeField] private AlienTierType _alienTier;
    [SerializeField] private AlienFamilyType _alienFamily;
    [SerializeField] private int _alienID;
    

    [Separator(1, 10)]
    [Header("Parent Aliens")]
    [SerializeField] private SO_Alien alien1;
    [SerializeField] private SO_Alien alien2;

    [Separator(1, 10)]
    [Header("Cost and Sell Values")]
    [SerializeField] private int costValue;
    public int sellValue;

    [Header("Days to Grow Alien")]
    [SerializeField] private int minDaysToGrow;
    [SerializeField] private int maxDaysToGrow;

    [Separator(1, 10)]
    [SerializeField] private AlienArt alienArt;
    [SerializeField] private Sprite alienSprite;
    [SerializeField] private Texture2D alienTexture;


    //Properties
    public SO_AliensContainer m_Container { get => _container; set => _container = value; }
    public SO_Alien m_ThisAlien { get => _thisAlien;}
    public string m_Name { get => _name; set => _name = value; }
    public AlienTierType m_AlienTier { get => _alienTier; set => _alienTier = value; }
    public Sprite m_AlienSprite { get => alienSprite;}
    public Texture2D m_AlienTexture { get => alienTexture; set => alienTexture = value; }
    public int m_AlienID { get => _alienID; set => _alienID = value; }
    public AlienFamilyType m_AlienFamily { get => _alienFamily; set => _alienFamily = value; }
    public GameObject m_AlienGO { get => alienGO; set => alienGO = value; }

    //
#if UNITY_EDITOR
    public void Initialize(SO_AliensContainer _alienContainer)
    {
        _container = _alienContainer;

        _thisAlien = this;
      
    }
#endif

    private void OnEnable()
    {
        //need to set this every time or else we get a nullreference error
        _thisAlien = this;
    }
}

[System.Serializable]
public class AlienArt
{
    public Animator idleAnim;
    public Animator wallkingAnim;
    
}

