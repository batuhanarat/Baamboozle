using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPopUp : Popup
{
    public virtual void SetData(IConfig config, Question question,CardUI cardUI)
    {

    }

    public virtual void Init()
    {

    }




}

public class QuestionAnswerQuestionPopUp : QuestionPopUp
{
    [SerializeField] private TMPro.TextMeshProUGUI questionText;

}