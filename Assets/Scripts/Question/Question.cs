using System;
using UnityEngine;

[Serializable]
public class Question
{
    public string questionText;
    //public Sprite image;
}

[Serializable]
public class TrueFalseQuestion : Question
{

    public int correctId;
    // 0 ise doğru cevap false,
    // 1 ise doğru cevap true
}

[Serializable]
public class SelfVerifiedQuestion : Question
{

}


[Serializable]
public class MultipleQuestion : Question
{
    public string[] answersText;
    public int correctAnswerId;
    public int optionCount;

}