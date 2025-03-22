using UnityEngine;
using UnityEngine.UI;

public class HaloEffect : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 30f; // Saniyede kaç derece döneceği
    [SerializeField] private bool clockwise = true; // Saat yönünde mi dönecek

    [Header("Target")]
    [SerializeField] private Image targetImage; // Döndürülecek Image bileşeni

    private void Start()
    {
        // Eğer targetImage atanmamışsa, bu GameObject'teki Image bileşenini kullan
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();

            // Eğer bu GameObject'te Image bileşeni yoksa uyarı ver
            if (targetImage == null)
            {
                Debug.LogWarning("ImageRotator: Hedef Image bulunamadı! Lütfen atayın veya bu GameObject'e bir Image ekleyin.");
                enabled = false; // Script'i devre dışı bırak
                return;
            }
        }
    }

    private void Update()
    {
        if (targetImage != null)
        {
            // Dönme yönünü belirle
            float direction = clockwise ? -1f : 1f;

            // Mevcut rotasyonu al
            Vector3 currentRotation = targetImage.rectTransform.localEulerAngles;

            // Z ekseni etrafında döndür (UI elemanları için Z ekseni kullanılır)
            currentRotation.z += direction * rotationSpeed * Time.deltaTime;

            // Yeni rotasyonu uygula
            targetImage.rectTransform.localEulerAngles = currentRotation;
        }
    }
}
