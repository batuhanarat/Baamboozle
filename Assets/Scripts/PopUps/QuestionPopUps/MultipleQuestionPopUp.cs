
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MultipleQuestionPopUp : QuestionPopUp
{
    [SerializeField] public TextMeshProUGUI questionText;
    [SerializeField] public TextMeshProUGUI pointText;
    private List<Button> options;

    [SerializeField] public GameObject optionPrefab;
    [SerializeField] public Transform buttonHolder;
    [SerializeField] public AudioClip trueSound;
    [SerializeField] public AudioClip falseSound;
    [SerializeField] public GameObject Correct;
    [SerializeField] public GameObject Wrong;
    private Color originalOptionColor = new Color(1f, 1f, 1f, 1f); // beyaz (varsayılan)



    private QuestionConfig questionConfig;
    private MultipleQuestion multipleQuestion;
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
            var multipleQuestion = (MultipleQuestion) question;

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

    private void SetQuestion(MultipleQuestion question)
    {
        multipleQuestion = question;

        questionText.text = multipleQuestion.questionText;
        pointText.text = questionConfig.point.ToString() + " puan";
        trueQuestionId = multipleQuestion.correctAnswerId;

        options = new List<Button>();

        var optionButtonGO = Instantiate(optionPrefab,buttonHolder);
        var optionButton = optionButtonGO.GetComponent<Button>();
        optionButton.GetComponentInChildren<TextMeshProUGUI>().SetText(multipleQuestion.answersText[multipleQuestion.correctAnswerId]);
        optionButton.onClick.AddListener(() => OnOptionPressed(multipleQuestion.correctAnswerId));
        options.Add(optionButton);

        var remaining = GameSettings.GetOptionCounts()-1;



        for( int i = 0 ; i< remaining ; i++)
        {
            if(i == multipleQuestion.correctAnswerId) continue;

            var optionButtonGO2 = Instantiate(optionPrefab,buttonHolder);
            var optionButton2 = optionButtonGO2.GetComponent<Button>();
            optionButton2.GetComponentInChildren<TextMeshProUGUI>().SetText(multipleQuestion.answersText[i]);
            int optionIndex = i;
            optionButton.onClick.AddListener(() => OnOptionPressed(optionIndex));
            options.Add(optionButton);
        }




        if(timerEnabled)
        {
            StartTimer();
        } else
        {
            GetComponentInChildren<Clock>().gameObject.SetActive(false);
        }

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
            team.UpdateScore(point);
            var source  = GetComponent<AudioSource>();
            source.clip = trueSound;
            source.Play();
            Correct.SetActive(true);
            Correct.GetComponent<Animator>().SetTrigger("CorrectTrigger");
        }
        else
        {
            var team = ServiceProvider.TeamManager.GetActiveTeam();
            team.UpdateScore(-point);
            var source  = GetComponent<AudioSource>();
            source.clip = falseSound;
            source.Play();
            Wrong.SetActive(true);
            Wrong.GetComponent<Animator>().SetTrigger("WrongTrigger");
        }
        ServiceProvider.TeamManager.ChangeTeam();
        ServiceProvider.ScoreManager.DecreaseCard();

    public void AfterOnOptionPressed(){
        ServiceProvider.TeamManager.ChangeTeam();
        card.DeactivateCard(); //ekranda bir şey kalıyorsa onları temizleriz
        Close();
    }

    public void TimeFinished()
    {
        var team = ServiceProvider.TeamManager.GetActiveTeam();
        team.UpdateScore(0);
        var source  = GetComponent<AudioSource>();
        source.clip = falseSound;
        source.Play();
        ServiceProvider.TeamManager.ChangeTeam();
        card.DeactivateCard();
        Close();
    }


}