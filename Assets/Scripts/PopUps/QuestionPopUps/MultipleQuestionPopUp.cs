
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleQuestionPopUp : QuestionPopUp
{
    [SerializeField] public TextMeshProUGUI questionText;
    [SerializeField] public TextMeshProUGUI questionTextWithImage;
    private TextMeshProUGUI currentQuestionText;
    [SerializeField] public TextMeshProUGUI pointText;
    [SerializeField] public TextMeshProUGUI pointTextWithImage;
    private TextMeshProUGUI currentPointText;
    private List<Button> options;
    [SerializeField] public GameObject optionPrefab;
    [SerializeField] public Transform buttonHolder;
    [SerializeField] public AudioClip trueSound;
    [SerializeField] public AudioClip falseSound;
    [SerializeField] public GameObject Correct;
    [SerializeField] public GameObject Wrong;
    private Color originalOptionColor = new Color(1f, 1f, 1f, 1f); // beyaz (varsayılan)
    [SerializeField] private Sprite sprite;

    [SerializeField] private GameObject normalTımer;
    [SerializeField] private GameObject timerForImage;
    [SerializeField] private GameObject normalBar;

    [SerializeField] private GameObject barforImage;

    [SerializeField] private GameObject reelImagego;
    [SerializeField] private Image reelImage;



    private QuestionConfig questionConfig;
    private  Question multipleQuestion;
    private int questionPoint => int.Parse(pointText.text.Replace(" puan", ""));

    private int questionPointStandart  => 60;


    private int trueQuestionId;

    private CardUI card;

    private bool timerEnabled = true;
    private bool decreasingScore = true;


    public override void SetData(IConfig config, Question question, CardUI cardUI)
    {
        if(config is QuestionConfig)
        {
            questionConfig = (QuestionConfig) config;
            this.card = cardUI;
        }

        QuestionType questionType = questionConfig.questionType;
        if( questionType == QuestionType.MULTIPLE_CHOICE)
        {
            var multipleQuestion = question;

            if(multipleQuestion == null)
            {
                Debug.Log("soru tipi hatalı geldi ");
            }
            GetConfigs();
            SetQuestion(multipleQuestion);

        }
    }

    private void GetConfigs()
    {
        var timeSettings =  GameSettings.GetTimeSetting();

        if(timeSettings == TimeSetting.NO_TIME)
        {
            timerEnabled = false;
        }
        if( timeSettings == TimeSetting.STANDART)
        {
            decreasingScore = false;
        }
    }

    private void SetQuestion(Question question)
    {
        multipleQuestion = question;
        if(question.hasImage)
        {
            Texture2D questionImage = ServiceProvider.QuestionManager.GetQuestionImage(question);
            normalTımer.SetActive(false);
            timerForImage.SetActive(true);

            normalBar.SetActive(false);
            barforImage.SetActive(true);

            currentQuestionText = questionTextWithImage;
            currentPointText = pointTextWithImage;

            reelImagego.SetActive(true);

            Sprite sprite = Sprite.Create(
                questionImage,
                new Rect(0, 0, questionImage.width, questionImage.height),
                new Vector2(0.5f, 0.5f)
            );

            // Assign the sprite to your UI Image component
            reelImage.sprite = sprite;
        }
         else
        {
            currentQuestionText =  questionText;
            currentPointText = pointText;
        }
        currentQuestionText.text = multipleQuestion.questionText;
        currentPointText.text = questionConfig.point.ToString() + " puan";
        trueQuestionId = multipleQuestion.correctAnswerId;


        options = new List<Button>();
        int optionCount = Mathf.Clamp(GameSettings.GetOptionCounts(), 2, 4); // 2, 3 veya 4 şık

        if(optionCount < 4 || question.hasImage)
        {
            buttonHolder.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
            buttonHolder.GetComponent<GridLayoutGroup>().constraintCount = 1;
        }


        List<int> allOptionIndexes = new List<int> { 0, 1, 2, 3 };

        List<int> activeOptionIndexes = new List<int> { trueQuestionId };

        List<int> wrongOptions = allOptionIndexes.Except(activeOptionIndexes).ToList();
        while (activeOptionIndexes.Count < optionCount && wrongOptions.Count > 0)
        {
            int randomIndex = Random.Range(0, wrongOptions.Count);
            activeOptionIndexes.Add(wrongOptions[randomIndex]);
            wrongOptions.RemoveAt(randomIndex);
        }

        activeOptionIndexes.Sort();

        foreach (int originalIndex in activeOptionIndexes)
        {
            var optionButtonGO = Instantiate(optionPrefab, buttonHolder);
            var optionButton = optionButtonGO.GetComponent<Button>();
            optionButton.GetComponentInChildren<TextMeshProUGUI>().text = multipleQuestion.answersText[originalIndex];

            optionButton.onClick.AddListener(() => OnOptionPressed(originalIndex));
            options.Add(optionButton);
        }

        if (timerEnabled) StartTimer();
        else GetComponentInChildren<Clock>().gameObject.SetActive(false);
    }

    public virtual void StartTimer()
    {
        Clock clock = GetComponentInChildren<Clock>();
        var initialTime = questionConfig.timeForQuestion;
        clock.SetTimerValue(initialTime,decreasingScore);
        clock.StartTimer();
    }

    public override void Init()
    {

    }


    public void OnOptionPressed(int optionId)
    {
        // Butona basılır basılmaz tüm opsiyonları devre dışı bıraktım. Buglar vardı.
        foreach (var optionButton in options)
        {
            optionButton.interactable = false;
        }

        if (optionId == trueQuestionId)
        {
            var team = ServiceProvider.TeamManager.GetActiveTeam();
            team.UpdateScore(questionPoint);
            var source  = GetComponent<AudioSource>();
            source.clip = trueSound;
            source.Play();
            Correct.SetActive(true);
            Correct.GetComponent<Animator>().SetTrigger("CorrectTrigger");
        }
        else
        {
            var team = ServiceProvider.TeamManager.GetActiveTeam();
            team.UpdateScore(-questionPoint);
            var source  = GetComponent<AudioSource>();
            source.clip = falseSound;
            source.Play();
            Wrong.SetActive(true);
            Wrong.GetComponent<Animator>().SetTrigger("WrongTrigger");
        }
    }
    public void AfterOnOptionPressed(){
        ServiceProvider.TeamManager.ChangeTeam();
        ServiceProvider.ScoreManager.DecreaseCard();
        card.DeactivateCard(); //ekranda bir şey kalıyorsa onları temizleriz
        Close();
    }

    public void TimeFinished()
    {
        var team = ServiceProvider.TeamManager.GetActiveTeam();
        ServiceProvider.ScoreManager.DecreaseCard();
        team.UpdateScore(0);
        var source  = GetComponent<AudioSource>();
        source.clip = falseSound;
        source.Play();
        ServiceProvider.TeamManager.ChangeTeam();
        card.DeactivateCard();
        Close();
    }


}