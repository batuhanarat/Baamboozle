
public class SpecialItemFactory : IProvidable
{

    public SpecialItemFactory()
    {
        ServiceProvider.Register(this);
    }


    public  SpecialCardBase CreateSpecialItem(SpecialCardType type)
    {
        SpecialCardBase specialItem = null;
        switch (type)
        {
            case SpecialCardType.BOMB:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Bomb>(AssetType.Bomb);
                break;
            case SpecialCardType.DOUBLE_POINTS:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<DoublePoints>(AssetType.DoublePoints);
                break;
            case SpecialCardType.ERASER:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Eraser>(AssetType.Eraser);
                break;
            case SpecialCardType.GOLD:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Gold>(AssetType.Gold);
                break;
            case SpecialCardType.MALLET:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Mallet>(AssetType.Mallet);
                break;
            case SpecialCardType.ROCKET:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Rocket>(AssetType.Rocket);
                break;
            case SpecialCardType.VIRUS:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Virus>(AssetType.Virus);
                break;
            case SpecialCardType.EXCHANGE:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Exchange>(AssetType.Exchange);
                break;
            case SpecialCardType.GIFT:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Gift>(AssetType.Gift);
                break;
            case SpecialCardType.KNIGHT:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Knight>(AssetType.Knight);
                break;
            case SpecialCardType.STRAWBERRY:
                specialItem = ServiceProvider.AssetLibrary.GetAsset<Strawberry>(AssetType.Strawberry);
                break;
        }

        return specialItem;
    }
}