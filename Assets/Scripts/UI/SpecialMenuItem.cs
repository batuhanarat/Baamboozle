using Ricimi;
using UnityEngine;

public class SpecialMenuItem : MonoBehaviour
{
    private bool canPresent = true;

    [SerializeField] public bool ısToggleForAll;

    public int GetPresentValue()
    {
        return canPresent ? 1 : 0;
    }

    public void OnClicked()
    {
        canPresent = !canPresent;
        if(ısToggleForAll)
        {
            ServiceProvider.SpecialItemSelector.DeselectAll();
        }
    }

    public void ForceToggle()
    {
        var swapper = GetComponentInChildren<SpriteSwapper>();
        swapper.SwapSprite();
        OnClicked();
    }
}