using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLayout : MonoBehaviour
{
    [SerializeField] public GameObject animationFalan;
    [Header("UI Canvas")]
    public RectTransform panelColumn;
    public Transform Board;
    private int GridSize;
    private int rowSize;
    private int columnSize;
    private const int SPEACIAL_TWEAKER = 5;
    private const int TOTAL_SPECIAL_COUNT = 11;
    private int questionCount;
    private int specialCount = 0;

    public TimeSetting TimeSetting;

    #region  Game Settings

        private QuestionType questionType;
        [SerializeField] public bool CanHaveSpecial;

    #endregion

    Dictionary<int , Vector2Int> gridSizeMapper = new Dictionary<int ,Vector2Int>{
        {7, new Vector2Int(7,1)},
        {14, new Vector2Int(7,2)},
        {21, new Vector2Int(7,3)},
        {28, new Vector2Int(7,4)},
        {8, new Vector2Int(4,2)},
        {12, new Vector2Int(4,3)},
        {15, new Vector2Int(3,5)},
        {16, new Vector2Int(4,4)},
        {18, new Vector2Int(6,3)},
        {20, new Vector2Int(5,4)},
        {24, new Vector2Int(6,4)},
        {25, new Vector2Int(5,5)},
        {30, new Vector2Int(6,5)},
        {32, new Vector2Int(8,4)},
        {35, new Vector2Int(7,5)},
        {36, new Vector2Int(6,6)},
        {40, new Vector2Int(8,5)},
        {42, new Vector2Int(6,7)},
        {45, new Vector2Int(9,5)},
        {48, new Vector2Int(6,8)},
    };

    [SerializeField] private GameObject cardPrefab;

    [SerializeField] List<CardConfig> cardConfigs;
    [SerializeField] List<CardConfig> questionConfigs;

    #region  Dictionaries
        Dictionary<SpecialCardType, CardConfig> specialCardDictionary;
        Dictionary<QuestionType, CardConfig> questionCardDictionary;

    #endregion

    #region Card Contents
        List<CardConfig> specialConfigs = new();
        List<Question> questionsInGame = new();
        List<object> allCards = new List<object>();

    # endregion

    public int[] specials = new int[TOTAL_SPECIAL_COUNT];
    public SpecialCardType[] specialEnums = new SpecialCardType[TOTAL_SPECIAL_COUNT];


    #region  Question Settings
        private CardConfig selectedQuestionConfig;
        private GameObject questionPrefab  => selectedQuestionConfig.PopUpPrefab;

    #endregion


    private List<SpecialCardType> ChooseSpecials(int gridSize)
    {
        specialCount = gridSize/SPEACIAL_TWEAKER;
        List<SpecialCardType> availableSpecials = new();

        for(int i = 0 ; i< TOTAL_SPECIAL_COUNT ; i++)
        {
            if(specials[i] == 1)
            {
                availableSpecials.Add(specialEnums[i]);
            }
        }

        List<SpecialCardType> selectedSpecials = new();

        while (selectedSpecials.Count < specialCount && availableSpecials.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableSpecials.Count);

            selectedSpecials.Add(availableSpecials[randomIndex]);

            availableSpecials.RemoveAt(randomIndex);
        }

        return selectedSpecials;
    }


    void Start()
    {

        GridSize = GameSettings.GetCardSize();
        GenerateBoard();
    }

    void Initialize()
    {
        rowSize = gridSizeMapper[GridSize].x;
        columnSize = gridSizeMapper[GridSize].y;
    }

    void ClearBoard()
    {
        for(int i = 0; i < Board.childCount; i++)
        {
            Destroy(Board.GetChild(i).gameObject);
        }
    }

    private void InitSpecialCardDict()
    {
        specialCardDictionary = new Dictionary<SpecialCardType, CardConfig>();
        foreach( var card in cardConfigs)
        {
            if (card.contentConfig is SpecialConfig)
            {
                var special = (SpecialConfig)card.contentConfig;
                specialCardDictionary.Add(special.specialCardType,card);
            }
        }
    }

    private void InitQuestionCardDict()
    {
        questionCardDictionary = new Dictionary<QuestionType, CardConfig>();
        foreach( var card in questionConfigs)
        {
            if (card.contentConfig is QuestionConfig)
            {
                var question = (QuestionConfig)card.contentConfig;
                questionCardDictionary.Add(question.questionType, card);
            }
        }
    }

    private void GetQuestions()
    {
        List<Question> questions = ServiceProvider.QuestionManager.allQuestions;
        int questionTypeNum = PlayerPrefs.GetInt("QuestionType", 0);

        if(questionTypeNum == 0)
        {
            questionType = QuestionType.MULTIPLE_CHOICE;
        } else if(questionTypeNum == 1)
        {
            questionType = QuestionType.TRUE_FALSE;
        } else
        {
             questionType = QuestionType.SELF_VERIFY;
        }

        selectedQuestionConfig = questionCardDictionary[questionType];

        questionCount = GridSize - specialCount;
        List<Question> selectedQuestions = new();
        List<Question> availableQuestions = new List<Question>(questions);

        while (selectedQuestions.Count < questionCount && availableQuestions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableQuestions.Count);
            selectedQuestions.Add(availableQuestions[randomIndex]);
            availableQuestions.RemoveAt(randomIndex);
        }

        foreach(var question in selectedQuestions)
        {
            var casted = (Question)question;
            questionsInGame.Add(casted);
            allCards.Add(casted);
        }
    }

    private List<object> GetRandomCards(int gridSize)
    {
        List<object> selectedCards = new List<object>();

        int cardCountToSelect = Mathf.Min(gridSize, allCards.Count);

        for (int i = 0; i < cardCountToSelect; i++)
        {
            if (allCards.Count == 0)
                break;

            int randomIndex = UnityEngine.Random.Range(0, allCards.Count);
            selectedCards.Add(allCards[randomIndex]);
            allCards.RemoveAt(randomIndex);
        }

        return selectedCards;
    }

    private List<SpecialCardType> GetSpecialsFromSettings()
    {
        CanHaveSpecial = !GameSettings.SpecialBlockAllowed;
        if(!CanHaveSpecial) return null;
        var availableSpecials = new  List<SpecialCardType>();

        if(GameSettings.isSpecialArrayChangedFromSettings)
        {
            var specialsFromSettingsArray = GameSettings.SpecialArray;
            for(int i = 0 ; i< TOTAL_SPECIAL_COUNT ; i++)
            {
                if(specialsFromSettingsArray[i] == 1)
                {
                    availableSpecials.Add(specialEnums[i]);
                }
            }

            return ChooseSpecialsFromSettings(GridSize,availableSpecials);
        }
        else
        {
            return ChooseSpecials(GridSize);

        }
    }


    private List<SpecialCardType> ChooseSpecialsFromSettings(int gridSize,  List<SpecialCardType> availableSpecials)
        {
            specialCount = gridSize/SPEACIAL_TWEAKER;

            List<SpecialCardType> selectedSpecials = new();

            while (selectedSpecials.Count < specialCount && availableSpecials.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableSpecials.Count);

                selectedSpecials.Add(availableSpecials[randomIndex]);

                availableSpecials.RemoveAt(randomIndex);
            }

            return selectedSpecials;
        }


    public void GenerateBoard()
    {
        ClearBoard();
        Initialize();
        List<SpecialCardType> specials = GetSpecialsFromSettings();

        if(CanHaveSpecial)
        {
            InitSpecialCardDict();
            foreach(var special in specials)
            {
                specialConfigs.Add(specialCardDictionary[special]);
                allCards.Add(specialCardDictionary[special]);
            }
        }
        Debug.Log("All cardsın size i speciallar eklenmiş  " +allCards.Count);

        ServiceProvider.ScoreManager.SetData(GameSettings.GetCardSize());
        InitQuestionCardDict();
        GetQuestions();
        Debug.Log("All cardsın size i questionlar eklenmiş  " +allCards.Count);
        List<object> selectedCards =  GetRandomCards(GridSize);
        Debug.Log("selected cards " + selectedCards.Count);

        RectTransform colParent;

        for(int colIndex = 0; colIndex < columnSize; colIndex++)
        {
            colParent = Instantiate(panelColumn, Board);

            for(int rowIndex = 0; rowIndex <rowSize; rowIndex++)
            {

                GameObject question = Instantiate(cardPrefab, colParent);
                int i = rowIndex * columnSize + colIndex + 1;
                question.GetComponent<CardUI>().SetQuestionNumber(i);

                int gameDataIndex = --i;

                var card = selectedCards[gameDataIndex];

                if(card is Question)
                {
                    var questionCard = (Question) card;
                    question.GetComponent<CardUI>().SetFieldForQuestion(selectedQuestionConfig.contentConfig, questionCard, selectedQuestionConfig.cardType, questionPrefab);

                } else if(card is CardConfig){

                    var specialCard = (CardConfig) card;
                    question.GetComponent<CardUI>().SetField(specialCard.contentConfig, specialCard.cardType, specialCard.PopUpPrefab);

                }

            }

        }
    }


}



