
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    
    private int _number;
    private bool _isBeneficial;
    [SerializeField] private Sprite darkblueSprite;
    [SerializeField] private Sprite lightblueSprite;
    [SerializeField] private TMPro.TextMeshProUGUI questionNumberText;
    [SerializeField] private Image cardImage;
    [SerializeField] public GameObject buttonHolder;
    [SerializeField] public GameObject shadow;
    [SerializeField] public GameObject text;
    [SerializeField] public Sprite giftSprite;
    [SerializeField] public GameObject giftEffect;
    [SerializeField] public GameObject giftEffect2;
    [SerializeField] public Sprite giftPrefab;
    [SerializeField] public GameObject rewardObject;
    [SerializeField] public GameObject backgroundPanel;
    [SerializeField] public GameObject sparkleHolder;
    [SerializeField] public GameObject haloEffect;
    [SerializeField] public GameObject openButton;
    [SerializeField] public GameObject fireAnimation;
    [SerializeField] public GameObject firstClickedAnimation;
    [SerializeField] public GameObject motion;
    [SerializeField] public AudioClip fireSound;
    [SerializeField] private AudioClip specialClickedSound2;
    [SerializeField] public GameObject SpecialGoodAnim;
    [SerializeField] public GameObject SpecialBadAnim;    
    [SerializeField] public GameObject Sparkle;
    [SerializeField] public GameObject Chest1;
    [SerializeField] public GameObject Chest3;
    [SerializeField] public GameObject Chest1_Original;
    [SerializeField] public GameObject Chest3_Original;
    [SerializeField] public GameObject Chest_Manager;
    [SerializeField] public GameObject QuestionNumber;
    [SerializeField] public GameObject Lock1;
    [SerializeField] public GameObject Lock3;
    [SerializeField] public AudioClip BombSoundGood;
    [SerializeField] public AudioClip BombSoundBad;
    [SerializeField] public AudioClip GoodSound;
    [SerializeField] public AudioClip BadSound;
    [SerializeField] public AudioClip WindSound;
    [SerializeField] public AudioClip FallingSound;


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
    /*public void SetForSpecialEffect()
    {

        StartCoroutine(SetAnimationAndStuff());

    }*/

    /*private IEnumerator SetAnimationAndStuff()
    {

        //Debug.Log("Buraya girdi ilk tıklanıldığında");
        firstClickedAnimation.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        firstClickedAnimation.SetActive(false);

        // Store original scale for later restoration
        Vector3 originalScale = buttonHolder.transform.localScale;
        var originalSprite = cardImage.sprite;


        Sequence shrinkSequence = DOTween.Sequence();

        // Add all objects to the shrink animation
        shrinkSequence.Join(text.transform.DOScale(0.1f, 0.25f));
        shrinkSequence.Join(shadow.transform.DOScale(0.1f, 0.25f));
        shrinkSequence.Join(buttonHolder.transform.DOScale(0.2f, 0.25f));
        shrinkSequence.Join(buttonHolder.transform.DOScale(0.2f, 0.25f));
        yield return shrinkSequence.WaitForCompletion();
        shadow.SetActive(false);
        text.SetActive(false);

        cardImage.type = Image.Type.Simple;
        cardImage.sprite = giftSprite;
        var rectTransform = cardImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        cardImage.preserveAspect = true;

        // Get canvas and parent references
        Canvas canvas = buttonHolder.GetComponentInParent<Canvas>();
        Transform originalParent = buttonHolder.transform.parent;
        buttonHolder.transform.SetParent(canvas.transform, true);

        // Set up background panel
        backgroundPanel.SetActive(true);
        RectTransform backgroundRect = backgroundPanel.GetComponent<RectTransform>();
        Vector2 originalSize = backgroundRect.sizeDelta;
        backgroundRect.sizeDelta = Vector2.zero;

        Sequence mySequence = DOTween.Sequence();

        // First grow back to original size
        mySequence.Append(buttonHolder.transform.DOScale(originalScale, 1.2f));



        //   var audioSource  = GetComponent<AudioSource>();
        //      audioSource.clip = specialClickedSound2; // Replace with your second audio clip variable
            //    audioSource.Play();


        yield return new WaitForSeconds(1.4f);
        Sequence newSequence = DOTween.Sequence();


        // Then grow larger and move to center while expanding background
        newSequence.Append(
            DOTween.Sequence()
                .Join(buttonHolder.transform.DOScale(new Vector3(1.8f, 1.4f, 1.2f), 1.3f))
                .Join(buttonHolder.transform.DOMove(Vector3.zero, 1f))
                .Join(backgroundRect.DOSizeDelta(originalSize, 1.3f))
        );

        newSequence.OnComplete(() => {
        Vector3 sparkleOriginalScale = sparkleHolder.transform.localScale;
        Vector3 haloOriginalScale = haloEffect.transform.localScale;
        Vector3 openButtonOriginalScale = openButton.transform.localScale;

        sparkleHolder.transform.localScale = sparkleOriginalScale * 0.2f;
        haloEffect.transform.localScale = haloOriginalScale * 0.2f;
        openButton.transform.localScale = openButtonOriginalScale * 0.2f;

        sparkleHolder.SetActive(true);
        haloEffect.SetActive(true);
        openButton.SetActive(true);

        sparkleHolder.transform.DOScale(sparkleOriginalScale, 0.5f);
        haloEffect.transform.DOScale(haloOriginalScale, 0.5f);
        openButton.transform.DOScale(openButtonOriginalScale, 0.5f);
    });

    }*/


    /*public void OnOpenChestButtonPressed()
    {
        giftEffect.SetActive(true);
        giftEffect2.SetActive(true);
        cardImage.GetComponent<Animator>().enabled = true;
        StartCoroutine(Wait5SecondAndOpenGift());
    }

    private IEnumerator Wait5SecondAndOpenGift()
    {
        yield return new WaitForSeconds(1f);
        //GetComponent<AudioSource>().clip = fireSound;
        //GetComponent<AudioSource>().Play();

        // Create a sequence to shrink all objects before deactivating
        Sequence shrinkSequence = DOTween.Sequence();

        // Add all objects to the shrink animation
        shrinkSequence.Join(sparkleHolder.transform.DOScale(0.1f, 0.2f));
        shrinkSequence.Join(giftEffect.transform.DOScale(0.1f, 0.2f));
    // shrinkSequence.Join(giftEffect2.transform.DOScale(0.1f, 0.2f));
        shrinkSequence.Join(haloEffect.transform.DOScale(0.1f, 0.2f));
        shrinkSequence.Join(openButton.transform.DOScale(0.1f, 0.2f));
        //shrinkSequence.Join(motion.transform.DOScale(0.1f, 0.2f));

        // Wait for the shrink animation to complete
        yield return shrinkSequence.WaitForCompletion();


        // Now deactivate all objects
        sparkleHolder.SetActive(false);
        giftEffect.SetActive(false);
        giftEffect2.SetActive(false);
        haloEffect.SetActive(false);
        openButton.SetActive(false);
        motion.SetActive(false);

        // Reset scales for future use
        sparkleHolder.transform.localScale = Vector3.one;
        giftEffect.transform.localScale = Vector3.one;
        giftEffect2.transform.localScale = Vector3.one;
        haloEffect.transform.localScale = Vector3.one;
        openButton.transform.localScale = Vector3.one;
        motion.transform.localScale = Vector3.one;

        // Activate fire animation
        // fireAnimation.SetActive(true);
     //yield return new WaitForSeconds(0.5F);
        // Get the Animator component
        //Animator animator = fireAnimation.GetComponent<Animator>();

        // Wait for the animation to complete
        // yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        // Deactivate fire animation
        //fireAnimation.SetActive(false);

        GetComponent<QuestionPopupOpener>().DecideWhatToDo();
    }*/

    public void DeactivateCard()
    {
        shadow.SetActive(false);
        buttonHolder.SetActive(false);
        text.SetActive(false);
        backgroundPanel.SetActive(false);

        sparkleHolder.SetActive(false);
        haloEffect.SetActive(false);
        giftEffect.SetActive(false);
        giftEffect2.SetActive(false);
        openButton.SetActive(false);


    }
    
    public void PlayEntranceAnimation(bool isBeneficial)
    {
        _isBeneficial = isBeneficial;

        if (isBeneficial)
        {
            SpecialGoodAnimation();
            BombAudioForGood();
        }
        else
        {
            SpecialBadAnimation();
            BombAudioForBad();
        }
    }
    
    public void ButtonDestroyed(){
        buttonHolder.SetActive(false);
        QuestionNumber.SetActive(false);
    }

    public void PanelOpened()
    {
        // Sahnedeki ana Canvas'ı bul
        GameObject mainCanvas = GameObject.Find("Canvas");
        
        if (mainCanvas != null && backgroundPanel != null)
        {
            backgroundPanel.transform.SetParent(mainCanvas.transform, false);
            RectTransform rect = backgroundPanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;

            backgroundPanel.SetActive(true);

            SparkleAnimation();

        }
        else
        {
            Debug.LogWarning("Main Canvas veya backgroundPanel bulunamadı!");
        }
    }

    public void PopupOpened()
    {
        Debug.Log("tetiklendim1");
        GetComponent<QuestionPopupOpener>().OpenPopUpWithAnimation();

        StartCoroutine(DisableLocksAfterDelay());
    }

    private IEnumerator DisableLocksAfterDelay()
    {
        yield return new WaitForSeconds(1.3f);

        if (Lock1 != null)
            Lock1.SetActive(false);

        if (Lock3 != null)
            Lock3.SetActive(false);
    }

    public void SpecialGoodAnimation(){
        SpecialGoodAnim.SetActive(true);
        SpecialGoodAnim.GetComponent<Animator>().SetTrigger("GoodAnimTrigger");
    }

    public void SpecialBadAnimation(){
        SpecialBadAnim.SetActive(true);
        SpecialBadAnim.GetComponent<Animator>().SetTrigger("BadAnimTrigger");
    }

    public void SparkleAnimation()
    {
        // Mevcut animasyonları kapat
        SpecialGoodAnim.SetActive(false);
        SpecialBadAnim.SetActive(false);

        // 1) Butonun dünya (world) pozisyonunu al
        Vector3 buttonWorldPos = buttonHolder.transform.position;

        Sparkle.SetActive(true);

        // 3) Sparkle'ı butonun konumuna yerleştir
        Sparkle.transform.position = buttonWorldPos;

        // 4) Başlangıçta çok küçük olsun (0)
        Sparkle.transform.localScale = Vector3.zero;

        RectTransform panelRect = backgroundPanel.GetComponent<RectTransform>();
        Vector3 panelCenterWorldPos = panelRect.TransformPoint(panelRect.rect.center);

        DG.Tweening.Sequence seq = DOTween.Sequence();

        seq.Append(
        Sparkle.transform
                .DOMove(panelCenterWorldPos, 1.5f)
                .SetEase(DG.Tweening.Ease.InOutQuad)
        );
        seq.Join(
        Sparkle.transform
                .DOScale(25f, 1.5f)
                .SetEase(DG.Tweening.Ease.InOutQuad)
        );

        seq.OnComplete(() =>
        {
            
            StartCoroutine(OpenChestsAfterDelay());

            // 8) Sonsuz yavaş dönme için ayrı bir Tween (Sequence dışında):
            Sparkle.transform
                .DORotate(
                    new Vector3(0f, 0f, 360f), 
                    64f,                        
                    RotateMode.FastBeyond360
                )
                .SetLoops(-1, DG.Tweening.LoopType.Incremental)
                .SetEase(DG.Tweening.Ease.Linear);
        });
    }

    private IEnumerator OpenChestsAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        AudioSource audioSource = GetComponent<AudioSource>();

        if (_isBeneficial)
        {
            Chest3.SetActive(true);

            audioSource.clip = FallingSound;
            audioSource.Play();

            // FallingSound süresi kadar bekle (örneğin 2 saniye)
            yield return new WaitForSeconds(1.3f);
            audioSource.Stop();
        }
        else
        {
            Chest1.SetActive(true);

            audioSource.clip = FallingSound;
            audioSource.Play();

            yield return new WaitForSeconds(1.6f);
            audioSource.Stop();
        }
    }


    public void BombAudioForBad()
    {
        StartCoroutine(PlayBombAndThenBadSurprise());
    }

    private IEnumerator PlayBombAndThenBadSurprise()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = BombSoundBad;
        audioSource.pitch = 2.5f;
        audioSource.Play();

        float waitTime = BombSoundBad.length / audioSource.pitch;
        yield return new WaitForSeconds(waitTime);

        BadSurpriseAudio();
    }

    public void BombAudioForGood()
    {
        StartCoroutine(PlayBombAndThenGoodSurprise());
    }

    private IEnumerator PlayBombAndThenGoodSurprise()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = BombSoundGood;
        audioSource.pitch = 2.5f;
        audioSource.Play();

        float waitTime = BombSoundGood.length / audioSource.pitch;
        yield return new WaitForSeconds(waitTime);

        GoodSurpriseAudio();
    }

    public void GoodSurpriseAudio(){
        GetComponent<AudioSource>().clip = GoodSound;
        GetComponent<AudioSource>().Play();
    }

    public void BadSurpriseAudio(){
        GetComponent<AudioSource>().clip = BadSound;
        GetComponent<AudioSource>().Play();
    }

    public void AfterChest1_Animation(){
        Chest1.SetActive(false);
        Chest1_Original.SetActive(true);
        Chest_Manager.SetActive(true);
    }

    public void AfterChest3_Animation(){
        Chest3.SetActive(false);
        Chest3_Original.SetActive(true);
        Chest_Manager.SetActive(true);
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
