using UnityEngine;

public class AssetLibrary : MonoBehaviour , IProvidable
{
    [Header("Prefabs")]
    [SerializeField] private GameObject BombPrefab;
    [SerializeField] private GameObject DoublePointsPrefab;
    [SerializeField] private GameObject EraserPrefab;
    [SerializeField] private GameObject GoldPrefab;
    [SerializeField] private GameObject MalletPrefab;
    [SerializeField] private GameObject RocketPrefab;
    [SerializeField] private GameObject VirusPrefab;
    [SerializeField] private GameObject ExchangePrefab;
    [SerializeField] private GameObject GiftPrefab;
    [SerializeField] private GameObject KnightPrefab;
    [SerializeField] private GameObject StrawberryPrefab;
    [SerializeField] public GameObject ANIMATION;

    [SerializeField] public GameObject WinPanel;


    [SerializeField] public Transform GamePopupRoot;



    private  void Awake()
    {
        ServiceProvider.Register(this);

    }
    public T GetAsset<T>(AssetType assetType) where T : class
    {
        var asset = GetAsset(assetType);
        return asset == null ? null : asset.GetComponent<T>();
    }

    private GameObject GetAsset(AssetType assetType)
    {
        return assetType switch
        {
            AssetType.Bomb => Instantiate(BombPrefab),
            AssetType.DoublePoints => Instantiate(DoublePointsPrefab),
            AssetType.Eraser => Instantiate(EraserPrefab),
            AssetType.Gold => Instantiate(GoldPrefab),
            AssetType.Mallet => Instantiate(MalletPrefab),
            AssetType.Virus => Instantiate(VirusPrefab),
            AssetType.Exchange => Instantiate(ExchangePrefab),
            AssetType.Rocket => Instantiate(RocketPrefab),
            AssetType.Gift => Instantiate(GiftPrefab),
            AssetType.Knight => Instantiate(KnightPrefab),
            AssetType.Strawberry => Instantiate(StrawberryPrefab),
            _ => null
        };
    }
}
public enum AssetType
{
    Bomb,
    DoublePoints,
    Eraser,
    Gold,
    Mallet,
    Rocket,
    Virus,
    Exchange,
    Gift,
    Knight,
    Strawberry
}
