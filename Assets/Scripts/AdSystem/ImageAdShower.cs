using UnityEngine;
using UnityEngine.UI;

public class ImageAdShower : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite sprite;
    public void ShowAdImage()
    {
        image.sprite = sprite;

        AspectRatioFitter aspectRatioFitter = GetComponent<AspectRatioFitter>();
        if (aspectRatioFitter == null)
        {
            aspectRatioFitter = gameObject.AddComponent<AspectRatioFitter>();
        }

        aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        aspectRatioFitter.aspectRatio = sprite.rect.width / sprite.rect.height;

        image.type = Image.Type.Simple;
        image.preserveAspect = true;
    }

}