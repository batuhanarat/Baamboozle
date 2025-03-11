using UnityEngine;

[CreateAssetMenu(menuName = "Baamboozle/QuestionConfig")]
public class QuestionConfig : IConfig
{
    public QuestionType questionType;
    public int point;
    public int timeForQuestion;

}



