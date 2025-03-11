
using System.Collections.Generic;
using TMPro;
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

    private QuestionConfig questionConfig;
    private MultipleQuestion multipleQuestion;
    private int questionPoint => int.Parse(pointText.text.Replace(" puan", ""));
    private int trueQuestionId;

    private CardUI card;


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
            SetQuestion(multipleQuestion);
        }
    }

    private void SetQuestion(MultipleQuestion question)
    {
        multipleQuestion = question;

        questionText.text = multipleQuestion.questionText;
        pointText.text = questionConfig.point.ToString();
        trueQuestionId = multipleQuestion.correctAnswerId;

        options = new List<Button>();
        for( int i = 0 ; i< multipleQuestion.optionCount ; i++)
        {
            var optionButtonGO = Instantiate(optionPrefab,buttonHolder);
            var optionButton = optionButtonGO.GetComponent<Button>();
            optionButton.GetComponentInChildren<TextMeshProUGUI>().SetText(multipleQuestion.answersText[i]);
            int optionIndex = i;
            optionButton.onClick.AddListener(() => OnOptionPressed(optionIndex));
            options.Add(optionButton);
        }

        StartTimer();

    }

    public virtual void StartTimer()
    {
        Clock clock = GetComponentInChildren<Clock>();
        var initialTime = questionConfig.timeForQuestion;
        clock.SetTimerValue(initialTime);
        clock.StartTimer();
    }

    public override void Init()
    {

    }

    public void OnOptionPressed(int optionId)
    {

        if(optionId == trueQuestionId)
        {
            var team = ServiceProvider.TeamManager.GetActiveTeam();
            team.UpdateScore(questionPoint);
            var source  = GetComponent<AudioSource>();
            source.clip = trueSound;
            source.Play();
        } else
        {
            var team = ServiceProvider.TeamManager.GetActiveTeam();
            team.UpdateScore(-questionPoint);
            var source  = GetComponent<AudioSource>();
            source.clip = falseSound;
            source.Play();
        }
        ServiceProvider.TeamManager.ChangeTeam();

        card.DeactivateCard();
        Close();
    }


}