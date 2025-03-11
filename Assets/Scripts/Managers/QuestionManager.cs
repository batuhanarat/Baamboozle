using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionList
{
    public List<MultipleQuestion> questions;
}

public class QuestionManager : MonoBehaviour , IProvidable
{
    private  void Awake()
    {
        ServiceProvider.Register(this);

    }

    public List<MultipleQuestion> allQuestions = new();

    void Start()
    {
        LoadQuestionsFromJSON();
    }

    void LoadQuestionsFromJSON()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Questions/MultipleQuestions");

        if (jsonTextAsset != null)
        {
            string jsonData = jsonTextAsset.text;

            QuestionList questionList = JsonUtility.FromJson<QuestionList>(jsonData);

            if (questionList != null && questionList.questions != null)
            {
                allQuestions = questionList.questions;
                Debug.Log("Toplam " + allQuestions.Count + " soru yuklendi.");

                // Sprite'ları yükle (eğer varsa)
               // LoadSpritesIfExists();
            }
            else
            {
                Debug.LogError("JSON dosyasi okunamadi veya bos.");
            }
        }
        else
        {
            Debug.LogError("Questions/MultipleQuestions.json dosyasi bulunamadi.");
        }
    }
/*
    void LoadSpritesIfExists()
    {

        foreach (MultipleQuestion question in allQuestions)
        {
            if (question.image != null)
            {
                if (!string.IsNullOrEmpty(question.image.name))
                {
                    Sprite loadedSprite = Resources.Load<Sprite>("QuestionImages/" + question.image.name);

                    if (loadedSprite != null)
                    {
                        question.image = loadedSprite;
                        Debug.Log("Sprite yuklendi: " + question.image.name);
                    }
                    else
                    {
                        Debug.LogWarning("Sprite bulunamadi: " + question.image.name);
                    }
                }
                else
                {
                    Debug.Log("Sprite adi tanimlanmamis.");
                }
            }
            else
            {
                // Image alanı null ise bir şey yapma
                Debug.Log("Soru icin resim alani tanimlanmamis: " + question.questionText);
            }
        }
    }

*/
    public MultipleQuestion GetQuestionByIndex(int index)
    {
        if (index >= 0 && index < allQuestions.Count)
        {
            return allQuestions[index];
        }

        Debug.LogError("Gecersiz soru indeksi: " + index);
        return null;
    }

    public MultipleQuestion GetRandomQuestion()
    {
        if (allQuestions.Count > 0)
        {
            int randomIndex = Random.Range(0, allQuestions.Count);
            return allQuestions[randomIndex];
        }

        Debug.LogError("Soru listesi bos!");
        return null;
    }
}