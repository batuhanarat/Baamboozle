using System.Collections.Generic;
using UnityEngine;

public class SpecialItemSelector : MonoBehaviour, IProvidable
{
    [SerializeField] private List<SpecialMenuItem> specialMenuItems;
    [SerializeField]  private SpecialMenuItem toggleAllItem;
    private const int TOTAL_SPECIAL_COUNT = 11;
    private int[] specialArray = new int[TOTAL_SPECIAL_COUNT];

    public void Awake()
    {
        ServiceProvider.Register(this);
    }

    public void DeselectAll()
    {
        for( int i = 0 ; i < TOTAL_SPECIAL_COUNT ; i++) {
            SpecialMenuItem item =  specialMenuItems[i];
            item.ForceToggle();
            specialArray[i] = 0;
        }
    }

    public void UpdateSelectedArray()
    {
        for( int i = 0 ; i < TOTAL_SPECIAL_COUNT ; i++) {
            SpecialMenuItem item =  specialMenuItems[i];
            specialArray[i] = item.GetPresentValue();
        }
        GameSettings.isSpecialArrayChangedFromSettings = true;
        GameSettings.SpecialArray = specialArray;
    }

    public int[] GetSpecialData()
    {
        return specialArray;
    }


}