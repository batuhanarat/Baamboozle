using System.Collections;
using DG.Tweening;
using UnityEngine;

public class QuestionPopupOpener : PopupOpener
{
    public GameObject newPopUpPrefab;
    public CardType CardType;
    public IConfig Config;
    public Question Question;
    private CardUI CardUI;
    private int counter = 0;
    private AudioClip popUpOpeningSound;

    [SerializeField] private AudioClip specialClickedSound1;

    public void SetConfigCardTypesAndPrefab(GameObject newPopUpPrefab, CardType cardType, IConfig config, CardUI cardUI)
    {
        this.newPopUpPrefab = newPopUpPrefab;
        this.CardType = cardType;
        this.Config = config;
        this.CardUI = cardUI;

        SpecialConfig specialConfigs =  (SpecialConfig) config;
        popUpOpeningSound = specialConfigs.openingSound;
    }

    public void SetConfigForQuestion(IConfig config, GameObject newPopUpPrefab, CardType cardType, Question question, CardUI cardUI)
    {
        this.newPopUpPrefab = newPopUpPrefab;
        this.CardType = cardType;
        this.Config = config;
        this.Question = question;
        this.CardUI = cardUI;
    }

    public void DecideWhatToDo()
    {
        if(CardType == CardType.QUESTION)
        {

            OpenPopup();
        }
        else
        {


            /*
            if(counter == 1)
            {
                GetComponent<AudioSource>().clip =  popUpOpeningSound;
                GetComponent<AudioSource>().Play();
                OpenPopUpWithAnimation();  // eğer butonlu isterlerse
            }
            else
            {
                counter++;
                var special = Config as SpecialConfig;
                GetComponent<CardUI>().PlayEntranceAnimation(special.isBeneficial);
            }
            */
            counter++;
            var special = Config as SpecialConfig;
            GetComponent<CardUI>().PlayEntranceAnimation(special.isBeneficial);
        }
    }

    public void OpenPopUpWithAnimation()
    {
        var popup = Instantiate(newPopUpPrefab) as GameObject;
        Debug.Log("tetiklendim2");

        popup.SetActive(false);
        popup.transform.SetParent(m_canvas.transform, false);

        RectTransform rectTransform = popup.GetComponent<RectTransform>();
/*
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);

        */

        // ⬇︎ Biraz daha AŞAĞIDAN başlasın (örneğin -500).
        rectTransform.anchoredPosition = new Vector2(0f, -500f);

        if(CardType == CardType.SPECIAL)
        {
            var playPopup = popup.GetComponent<SpecialPopUp>();
            playPopup.SetConfig(CardType, Config, CardUI);
            playPopup.Init();
        }

        Animator animator = popup.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        popup.transform.localScale = Vector3.one * 0.3f; // Küçük başlasın
        popup.SetActive(true);

        // ✅ SANDIK AÇILDIKTAN SONRA kısacık bekleme + animasyon
        StartCoroutine(DelayThenAnimate(popup, rectTransform));
    }

    // 🔥 EKLENEN COROUTINE:
    private IEnumerator DelayThenAnimate(GameObject popup, RectTransform rectTransform)
    {
        // Sandık tam açıldıktan sonra 0.5 sn bekle (isteğe göre ayarla)
        yield return new WaitForSeconds(0.01f);

        // Beklemeden sonra yukarı süzülme + büyüme animasyonu başlasın
        StartCoroutine(AnimateMoveThenScale(popup.transform, rectTransform));
    }

    private IEnumerator AnimateMoveThenScale(Transform popupTransform, RectTransform rectTransform)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        // ⬇︎ Yukarı gideceği bitiş noktası (örneğin -300). Dilersen -350 vs. de yapabilirsin.
        Vector2 endPos = new Vector2(0f, 0f);

        Vector3 scale = popupTransform.localScale;

        float moveDuration = 1f;
        float t = 0;

        rectTransform.localScale = scale;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, t / moveDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;

        // Sonra eskisi gibi büyüsün
        StartCoroutine(AnimatePopupScaleDelayed(popupTransform));
    }

    private IEnumerator AnimatePopupScaleDelayed(Transform popupTransform)
    {
        yield return new WaitForEndOfFrame();

        /*
        if (popupTransform.localScale != Vector3.zero)
        {
            popupTransform.localScale = Vector3.zero;
            Debug.Log("Scale was reset to zero again!");
        }
        */

        float duration = 1.2f; // Büyüme hızı (ayarla)
        float startTime = Time.time;

        Vector3 startScale = popupTransform.localScale;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            t = 1 - Mathf.Pow(1 - t, 3); // Ease out cubic
            popupTransform.localScale = Vector3.Lerp(startScale, Vector3.one, t);
            yield return null;
        }

        popupTransform.localScale = Vector3.one;

        Animator animator = popupTransform.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
        GetComponent<CardUI>().DeactivateCard();


        popupTransform.transform.SetParent(ServiceProvider.AssetLibrary.GamePopupRoot);

    }

    public override void OpenPopup()
    {
        var rootTransform = ServiceProvider.AssetLibrary.GamePopupRoot;

        if(rootTransform == null)
        {
            Debug.Log("bu transform boş kanka");
        }
        var popup = Instantiate(newPopUpPrefab);
        popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.SetParent(m_canvas.transform, false);

        if(CardType == CardType.SPECIAL)
        {
            var playPopup = popup.GetComponent<SpecialPopUp>();
            playPopup.SetConfig(CardType, Config, CardUI);
            playPopup.Init();
            playPopup.transform.SetParent(rootTransform);

        }
        else if(CardType == CardType.QUESTION)
        {
            var playPopup = popup.GetComponent<QuestionPopUp>();
            playPopup.SetData(Config, Question, CardUI);
            playPopup.Init();
            playPopup.transform.SetParent(rootTransform);

        }
    }
}
