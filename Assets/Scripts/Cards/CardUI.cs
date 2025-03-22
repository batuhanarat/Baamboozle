
using System.Collections;
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



            StartCoroutine(SetAnimationAndStuff());

        }

    private IEnumerator SetAnimationAndStuff()
    {


    firstClickedAnimation.SetActive(true);
    yield return new WaitForSeconds(0.7f);
    firstClickedAnimation.SetActive(false);

    // Store original scale for later restoration
    Vector3 originalScale = buttonHolder.transform.localScale;
    var originalSprite = cardImage.sprite;

    // Step 1: Shrink the buttonHolder

        Sequence shrinkSequence = DOTween.Sequence();

        // Add all objects to the shrink animation
        shrinkSequence.Join(text.transform.DOScale(0.1f, 0.25f));
        shrinkSequence.Join(shadow.transform.DOScale(0.1f, 0.25f));
        shrinkSequence.Join(buttonHolder.transform.DOScale(0.2f, 0.25f));
        yield return shrinkSequence.WaitForCompletion();
        shadow.SetActive(false);
        text.SetActive(false);





        // Step 2: Change the image
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

        // Step 3: Create a sequence with multiple animations
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
    // Store original scales
    Vector3 sparkleOriginalScale = sparkleHolder.transform.localScale;
    Vector3 haloOriginalScale = haloEffect.transform.localScale;
    Vector3 openButtonOriginalScale = openButton.transform.localScale;

    // Set them active but with small initial scale
    sparkleHolder.transform.localScale = sparkleOriginalScale * 0.2f;
    haloEffect.transform.localScale = haloOriginalScale * 0.2f;
    openButton.transform.localScale = openButtonOriginalScale * 0.2f;

    // Activate them
    sparkleHolder.SetActive(true);
    haloEffect.SetActive(true);
    openButton.SetActive(true);

    // Animate them to their original scale
    sparkleHolder.transform.DOScale(sparkleOriginalScale, 0.5f);
    haloEffect.transform.DOScale(haloOriginalScale, 0.5f);
    openButton.transform.DOScale(openButtonOriginalScale, 0.5f);
});

}
        public void OnOpenChestButtonPressed()
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
}

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

    public void SetField(IConfig specialConfig,CardType cardType, GameObject questionPopup)
    {
        GetComponent<QuestionPopupOpener>().SetConfigCardTypesAndPrefab(questionPopup, cardType, specialConfig,this);

    }
    public void SetFieldForQuestion(IConfig questionConfig, Question question , CardType cardType, GameObject questionPopup)
    {
        GetComponent<QuestionPopupOpener>().SetConfigForQuestion(questionConfig,questionPopup, cardType, question,this);

    }
}
