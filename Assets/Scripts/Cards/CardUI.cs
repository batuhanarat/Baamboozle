
using DG.Tweening;
using Ricimi;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private int _number;

    [SerializeField] private Sprite darkblueSprite;
    [SerializeField] private Sprite lightblueSprite;
    [SerializeField] private TMPro.TextMeshProUGUI questionNumberText;
    [SerializeField] private Image cardImage;

    [SerializeField] public GameObject button;
    [SerializeField] public GameObject shadow;
    [SerializeField] public GameObject text;
    [SerializeField] public Sprite giftSprite;

    [SerializeField] public GameObject sparkleHolder;

    public void SetQuestionNumber(int number)
    {
        _number = number;
        questionNumberText.text = number.ToString();
        SetSprite(number);
    }
    public void SetSprite(int number)
    {
        if(number % 2 == 0)
        {
            cardImage.sprite = lightblueSprite;

        }
    }


public void SetForSpecialEffect()
{
    // Önce mevcut sprite'ı küçültme işlemi
    var image = button.GetComponent<Image>();
    var originalSprite = image.sprite; // Mevcut sprite'ı saklayalım

    // Küçültme animasyonu (0.3 saniye sürecek şekilde)
    button.transform.DOScale(0.2f, 0.3f).OnComplete(() => {
        // Küçültme tamamlandıktan sonra yeni ayarlar
        shadow.SetActive(false);
        text.SetActive(false);

        // Yeni sprite'ı ayarla
        image.type = Image.Type.Simple;
        image.sprite = giftSprite;

        // RectTransform ayarları
        var rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        image.preserveAspect = true;

        // Butonu küçük başlat ve büyütme animasyonunu başlat
        button.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        button.transform.DOScale(new Vector3(1.4f, 1f, 1f), 0.3f).OnComplete(() => {
            // Animasyon tamamlandıktan sonra
            var ANIMATION = button.GetComponent<Animator>();
            ANIMATION.enabled = true;
            sparkleHolder.SetActive(true);
        });
    });
}

    public void DeactivateCard()
    {
        shadow.SetActive(false);
        button.SetActive(false);
        text.SetActive(false);
        sparkleHolder.SetActive(false);
    }

    public void SetField(IConfig specialConfig,CardType cardType, GameObject questionPopup)
    {
        GetComponent<QuestionPopupOpener>().SetConfigCardTypesAndPrefab(questionPopup, cardType, specialConfig,this);

    }
    public void SetFieldForQuestion(IConfig questionConfig, Question question , CardType cardType, GameObject questionPopup)
    {
        GetComponent<QuestionPopupOpener>().SetConfigForQuestion(questionConfig,questionPopup, cardType, question,this);

    }
}
