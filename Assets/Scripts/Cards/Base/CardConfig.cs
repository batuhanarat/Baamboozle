using UnityEngine;

[CreateAssetMenu(menuName = "Baamboozle/CardConfig")]
public class CardConfig : ScriptableObject
{
    public GameObject PopUpPrefab;
    public IConfig contentConfig;
    public CardType cardType;
}


public class IConfig : ScriptableObject
{

}

