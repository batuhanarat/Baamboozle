using UnityEngine;
using UnityEngine.UI;

public class SpecialPopUp : Popup
    {

        [SerializeField] private TMPro.TextMeshProUGUI description;
        [SerializeField] private TMPro.TextMeshProUGUI questionText;
        [SerializeField] private Button executeButton;
        [SerializeField] private Image iconSprite;

        private SpecialConfig specialConfig;
        private QuestionConfig questionConfig;
        protected SpecialCardBase specialCard;
        private bool isPressing = false;

        private bool IsSpecial = false;
        protected CardUI card;

        protected TMPro.TextMeshProUGUI GetDescription()
        {
            return description;
        }

        protected void SetDescription(string text)
        {
            description.text = text;
        }

        private void SetSpecialConfig(SpecialConfig config)
        {
            specialConfig = config;
            description.text = config.description;
            questionText.text = config.specialName;
            iconSprite.sprite = config.sprite;
        }


        public void SetConfig(CardType cardType , IConfig config, CardUI cardUI)
        {
            if( cardType == CardType.SPECIAL)
            {
                this.card = cardUI;
                IsSpecial = true;
                SetSpecialConfig((SpecialConfig)config);
                var factory = ServiceProvider.SpecialItemFactory;
                specialCard = factory.CreateSpecialItem(specialConfig.specialCardType);
                specialCard.transform.SetParent(transform);
            }
        }

        public void OnExecuteButtonPressed()
        {
            if(isPressing) return;
            isPressing = true;
            if(IsSpecial)
            {
                specialCard.Execute();
            }
            card.DeactivateCard();
            Close();
        }
        public virtual void Init()
        {

        }

}
