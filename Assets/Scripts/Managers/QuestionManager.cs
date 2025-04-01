using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class QuestionsInfo
{
    public int questionType;
    public List<Question> questions;
}

public class QuestionManager :   IProvidable
{
    public QuestionManager()
    {
        ServiceProvider.Register(this);
    }

    public List<Question> allQuestions = new();

    private const string KEY_PREFIX = "Questions_Part_";
    private const int MAX_SIZE_PER_KEY = 500000;

    private const string questionKey =  "questionsAreLoaded";

    public bool isEverythingAllright = false;



    public void DeletePrefsAndLoadQuestions(Action onSuccess, Action OnFail)
    {
       TryGetQuestionsFromApiAndDeleteOldOnesIfSucceded(); // bu da resources dan alıyor
    }


    public void LoadQuestionsIfNeeded()
    {
        if(PlayerPrefs.HasKey(questionKey)) {

            if(PlayerPrefs.GetInt(questionKey) == 1)
            {
                if (LoadQuestionsFromPlayerPrefs())
                {
                    Debug.Log("Cache den alıyorum");

                    isEverythingAllright = true;
                    return;
                }
                else {
                    Debug.LogError("player prefste var gözüküyordu ama dönemedi");
                }
            }
        }
        Debug.Log("Cache de yok api den alıyorum");

        LoadQuestionsFromApi(); // şimdilik resources dan okuyor
    }


    public int CalculateJsonSize(string json)
    {
        return Encoding.UTF8.GetByteCount(json);
    }


    public void SaveQuestionsToPlayerPrefs()
    {
        QuestionsInfo questionInfo = new QuestionsInfo();
        questionInfo.questions = allQuestions;
        string fullJson = JsonUtility.ToJson(questionInfo);

        int totalSize = CalculateJsonSize(fullJson);
        Debug.Log($"JSON toplam boyutu: {totalSize} bytes ({totalSize / 1024f} KB)");

        if (totalSize <= MAX_SIZE_PER_KEY)
        {
            Debug.Log("JSON tek parça olarak kaydediliyor.");
            PlayerPrefs.SetString(KEY_PREFIX + "0", fullJson);
            PlayerPrefs.SetInt(KEY_PREFIX + "Count", 1);
            PlayerPrefs.Save();
            return;
        }

        // Veriyi parçalara böl
        Debug.Log($"JSON boyutu büyük olduğu için parçalara bölünüyor (max: {MAX_SIZE_PER_KEY / 1024f} KB)");

        int questionCount = allQuestions.Count;
        int questionsPerKey = Mathf.Max(1, questionCount / Mathf.CeilToInt((float)totalSize / MAX_SIZE_PER_KEY));

        int partCount = 0;
        for (int i = 0; i < questionCount; i += questionsPerKey)
        {
            QuestionsInfo partInfo = new QuestionsInfo();
            partInfo.questions = new List<Question>();

            // Bu parçaya ait soruları ekle
            int endIndex = Mathf.Min(i + questionsPerKey, questionCount);
            for (int j = i; j < endIndex; j++)
            {
                partInfo.questions.Add(allQuestions[j]);
            }

            string partJson = JsonUtility.ToJson(partInfo);
            int partSize = CalculateJsonSize(partJson);

            Debug.Log($"Parça {partCount} boyutu: {partSize} bytes ({partSize / 1024f} KB), Soru sayısı: {partInfo.questions.Count}");

            PlayerPrefs.SetString(KEY_PREFIX + partCount, partJson);
            partCount++;
        }

        // Toplam parça sayısını kaydet
        PlayerPrefs.SetInt(KEY_PREFIX + "Count", partCount);
        PlayerPrefs.Save();
        Debug.Log($"JSON {partCount} parçaya bölünerek kaydedildi.");
    }

    public bool LoadQuestionsFromPlayerPrefs()
    {
        // Parça sayısını kontrol et
        if (!PlayerPrefs.HasKey(KEY_PREFIX + "Count"))
        {
            Debug.Log("Kaydedilmiş veri bulunamadı.");
            return false;
        }

        int partCount = PlayerPrefs.GetInt(KEY_PREFIX + "Count");
        allQuestions.Clear();

        for (int i = 0; i < partCount; i++)
        {
            string partKey = KEY_PREFIX + i;
            if (!PlayerPrefs.HasKey(partKey))
            {
                Debug.LogError($"Parça {i} bulunamadı!");
                continue;
            }

            string partJson = PlayerPrefs.GetString(partKey);
            QuestionsInfo partInfo = JsonUtility.FromJson<QuestionsInfo>(partJson);

            if (partInfo != null && partInfo.questions != null)
            {
                allQuestions.AddRange(partInfo.questions);
                Debug.Log($"Parça {i}: {partInfo.questions.Count} soru yüklendi.");
            }
        }

        Debug.Log($"Toplam {allQuestions.Count} soru yüklendi ({partCount} parça).");
        return allQuestions.Count > 0;
    }


    public void TryGetQuestionsFromApiAndDeleteOldOnesIfSucceded()
    {
        //resources yerine api den al
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Questions/MultipleQuestions");

        if (jsonTextAsset != null)
        {
            string jsonData = jsonTextAsset.text;

            QuestionsInfo questionList = JsonUtility.FromJson<QuestionsInfo>(jsonData);

            if (questionList != null && questionList.questions != null)
            {
                allQuestions = questionList.questions;
                Debug.Log("Toplam " + allQuestions.Count + " soru yuklendi.");
                PlayerPrefs.DeleteAll();
                SaveQuestionsToPlayerPrefs();

                PlayerPrefs.SetInt(questionKey,1);
                PlayerPrefs.Save();
                isEverythingAllright = true;
;
            }
            else
            {
                Debug.Log("JSON dosyasi okunamadi veya bos.");
                return;
            }
        }
        else
        {
            Debug.LogError("Questions/MultipleQuestions.json dosyasi bulunamadi.");
        }
    }

    public void LoadQuestionsFromApi()
    {
        //resources yerine api den al
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Questions/MultipleQuestions");

        if (jsonTextAsset != null)
        {
            string jsonData = jsonTextAsset.text;

            QuestionsInfo questionList = JsonUtility.FromJson<QuestionsInfo>(jsonData);

            if (questionList != null && questionList.questions != null)
            {
                allQuestions = questionList.questions;
                Debug.Log("Toplam " + allQuestions.Count + " soru yuklendi.");
                SaveQuestionsToPlayerPrefs();

                PlayerPrefs.SetInt(questionKey,1);
                PlayerPrefs.Save();
                isEverythingAllright = true;
;
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

    public Question GetQuestionByIndex(int index)
    {
        if (index >= 0 && index < allQuestions.Count)
        {
            return allQuestions[index];
        }

        Debug.LogError("Gecersiz soru indeksi: " + index);
        return null;
    }

    public Question GetRandomQuestion()
    {
        if (allQuestions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, allQuestions.Count);
            return allQuestions[randomIndex];
        }

        Debug.LogError("Soru listesi bos!");
        return null;
    }
}