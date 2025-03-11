using UnityEngine;

[CreateAssetMenu(menuName = "Baamboozle/SpecialConfig")]
public class SpecialConfig :  IConfig
{
    public string specialName;
    public SpecialCardType specialCardType;
    public GameObject iconPrefab;
    public Sprite sprite;
    public string description;
    public bool isBeneficial;
    public AudioClip openingSound;

}