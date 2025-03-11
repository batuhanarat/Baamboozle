

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

        [SerializeField] private AudioClip specialClickedSound;


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

                    OpenPopup();
                } else
                {
                    counter++;
                    GetComponent<AudioSource>().clip =  specialClickedSound;
                    GetComponent<CardUI>().SetForSpecialEffect();

                }
            }
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

