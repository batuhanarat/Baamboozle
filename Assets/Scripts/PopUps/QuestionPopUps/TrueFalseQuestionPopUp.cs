
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrueFalseQuestionPopUp : QuestionPopUp
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

    [SerializeField] private GameObject normalTımer;
    [SerializeField] private GameObject timerForImage;
    [SerializeField] private GameObject normalBar;

    [SerializeField] private GameObject barforImage;

    [SerializeField] private GameObject reelImagego;
    [SerializeField] private Image reelImage;



    private QuestionConfig questionConfig;
    private  Question trueFalseQuestion;
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
        if( questionType == QuestionType.TRUE_FALSE)
        {
            var trueFalseQuestion = question;

            if(trueFalseQuestion == null)
            {
                Debug.Log("soru tipi hatalı geldi ");
            }
            GetConfigs();
            SetQuestion(trueFalseQuestion);
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
        trueFalseQuestion = question;
        if(question.hasImage)
        {
            Texture2D questionImage = GetComponent<QuestionManager>().GetQuestionImage(question);

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
        {
            currentQuestionText =  questionText;
            currentPointText = pointText;
        }
        currentQuestionText.text = trueFalseQuestion.questionText;
        currentPointText.text = questionConfig.point.ToString() + " puan";
        trueQuestionId = trueFalseQuestion.correctAnswerId;



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

    //truequestionıd 0 ise doğru cevap false
     public void OnFalsePressed()
    {
        var questionId = 0;
        if(trueQuestionId == questionId)
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

    public void OnTruePressed()
    {
        var questionId = 1;
        if(trueQuestionId == questionId)
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