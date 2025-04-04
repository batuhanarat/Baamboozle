using System;

[Serializable]
public class Question
{
    public string questionText;
    public string[] answersText;
    public int correctAnswerId;
    public bool hasImage;
    public string imagePath;
}