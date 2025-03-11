using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrueFalseQuestionPopUp : QuestionPopUp
{
    [SerializeField] public TextMeshProUGUI questionText;
    [SerializeField] public List<Button> options;
    [SerializeField] public AudioClip trueSound;
    [SerializeField] public AudioClip falseSound;


    private int questionPoint;
    private int trueQuestionId;
    private QuestionConfig questionConfig;


    private void SetQuestionConfig(QuestionConfig config)
    {
        questionConfig = config;
        /*
        questionText.text = config.question;
        questionPoint = config.point;
        trueQuestionId = config.correctAnswer;
        for( int i = 0 ; i< config.answer.Length ; i++)
        {
            options[i].GetComponentInChildren<TextMeshProUGUI>().SetText(config.answer[i]);
            options[i].onClick.AddListener( () => OnOptionPressed(i));
        }
        */
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
        Close();
    }


}