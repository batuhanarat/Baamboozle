

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
                if(counter == 1)
                {
                    GetComponent<AudioSource>().clip =  popUpOpeningSound;
                    Debug.Log("COUNTER 1");
                    GetComponent<AudioSource>().Play();
                    OpenPopUpWithAnimation();
                } else
                {
                    counter++;
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.clip = specialClickedSound1;

                    GetComponent<CardUI>().SetForSpecialEffect();
                }
            }
        }

public void OpenPopUpWithAnimation()
{
    var popup = Instantiate(newPopUpPrefab) as GameObject;

    // Popup'ı başlangıçta aktif etme
    popup.SetActive(false);

    // Parent'a ekle
    popup.transform.SetParent(m_canvas.transform, false);

    if(CardType == CardType.SPECIAL)
    {
        var playPopup = popup.GetComponent<SpecialPopUp>();
        playPopup.SetConfig(CardType, Config, CardUI);
        playPopup.Init();
    }

    // Animator kontrol et ve devre dışı bırak
    Animator animator = popup.GetComponent<Animator>();
    if (animator != null)
    {
        animator.enabled = false;
    }

    // Ölçeği sıfırla ve popup'ı aktif et
    popup.transform.localScale = Vector3.zero;
    popup.SetActive(true);

    // LateUpdate'te çalışacak şekilde animasyonu başlat
    StartCoroutine(AnimatePopupScaleDelayed(popup.transform));
}

private IEnumerator AnimatePopupScaleDelayed(Transform popupTransform)
{
    // Bir frame bekle (LateUpdate'ten sonra)
    yield return new WaitForEndOfFrame();

    // Ölçeğin hala sıfır olduğunu kontrol et
    if (popupTransform.localScale != Vector3.zero)
    {
        popupTransform.localScale = Vector3.zero;
        Debug.Log("Scale was reset to zero again!");
    }

    float duration = 2f;
    float startTime = Time.time;

    while (Time.time < startTime + duration)
    {
        float t = (Time.time - startTime) / duration;
        t = 1 - Mathf.Pow(1 - t, 3); // Ease out cubic

        popupTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

        yield return null;
    }

    popupTransform.localScale = Vector3.one;

    // Animator'ı tekrar etkinleştir (gerekirse)
    Animator animator = popupTransform.GetComponent<Animator>();
    if (animator != null)
    {
        animator.enabled = true;
    }
    GetComponent<CardUI>().DeactivateCard();

}
        public override void OpenPopup()
        {
            var popup = Instantiate(newPopUpPrefab) as GameObject;
            popup.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.SetParent(m_canvas.transform, false);

            if(CardType == CardType.SPECIAL)
            {
                var playPopup = popup.GetComponent<SpecialPopUp>();
                playPopup.SetConfig(CardType, Config, CardUI);
                playPopup.Init();
            }
            else if(CardType == CardType.QUESTION)
            {
                var playPopup = popup.GetComponent<QuestionPopUp>();
                playPopup.SetData(Config, Question, CardUI);
                playPopup.Init();
            }


        }
    }

