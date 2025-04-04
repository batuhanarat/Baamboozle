using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class QuestionsInfo
{
    public int questionType;
    public List<Question> questions;
}
[System.Serializable]
public class ApiResponse
{
    public int id;
    public string name;
    public string code;
    public string question_type;
    public ApiQuestion[] questions;
}

[System.Serializable]
public class ApiQuestion
{
    public int id;
    public string question_text;
    public string question_type;
    public string difficulty;
    public string image_path;
    public ApiAnswer[] answers;
}

[System.Serializable]
public class ApiAnswer
{
    public int id;
    public string answer_text;
    public bool is_correct;
}

public class QuestionManager : MonoBehaviour, IProvidable
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private UpdatePanel updatePanel;

    private const string API_URL = "https://ariyayin.broosmedia.com/public/api/unity/question-groups/code/";
    private const string LOGIN_KEY = "HasLoggedIn";
    private const string QUESTIONS_KEY = "QuestionsData";
    private const string QUESTIONS_COUNT_KEY = "QuestionsCount";
    private const string QUESTION_TYPE_KEY = "QuestionType";
    private const int MAX_PREFS_SIZE = 1000000;

    public List<Question> allQuestions = new();
    private Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();


    void Awake()
    {
        ServiceProvider.Register(this);
    }

    private void Start()
    {
        CheckLoginStatus();
    }
    private void CheckLoginStatus()
    {
        if (PlayerPrefs.HasKey(LOGIN_KEY) && PlayerPrefs.GetInt(LOGIN_KEY) == 1)
        {
            // User already logged in
            loginPanel.SetActive(false);
            LoadQuestionsFromPlayerPrefs();
        }
        else
        {
            // User needs to login
            loginPanel.SetActive(true);
        }
    }

    public void OnLoginButtonClicked(string codeInput)
    {
        StartCoroutine(FetchQuestions(codeInput));
    }
    public void UpdateQuestions(string code)
    {
        StartCoroutine(UpdateQuestionsCoroutine(code));
    }
    private IEnumerator UpdateQuestionsCoroutine(string code)
    {
    string url = API_URL + code;

    using (UnityWebRequest request = UnityWebRequest.Get(url))
    {
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            updatePanel = FindFirstObjectByType<UpdatePanel>();

            updatePanel.ShowError("Güncelleme hatası: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            try
            {
                // Process the response but don't save to PlayerPrefs yet
                QuestionsInfo newQuestionsInfo = ProcessApiResponseWithoutSaving(jsonResponse);

                if (newQuestionsInfo != null && newQuestionsInfo.questions != null && newQuestionsInfo.questions.Count > 0)
                {
                    // Now that we've successfully retrieved and processed the new data,
                    // we can safely clear the old data
                    ClearOldData();

                    // Save the new data
                    SaveQuestionsToPlayerPrefs(newQuestionsInfo);

                    // Update our working copy
                    allQuestions = newQuestionsInfo.questions;

                    // Download and cache images
                    StartCoroutine(DownloadAllImages(newQuestionsInfo.questions));
                    updatePanel = FindFirstObjectByType<UpdatePanel>();

                    updatePanel.ShowSuccess("Sorular başarıyla güncellendi!");

                }
                else
                {
                    updatePanel = FindFirstObjectByType<UpdatePanel>();

                    updatePanel.ShowError("Yeni sorular alınamadı veya boş veri döndü.");
                }
            }
            catch (Exception e)
            {
                updatePanel = FindFirstObjectByType<UpdatePanel>();

                updatePanel.ShowError("Veri işlenemedi: " + e.Message);
                Debug.LogError("JSON Processing Error during update: " + e.Message);
            }
        }
    }
    }
    private QuestionsInfo ProcessApiResponseWithoutSaving(string jsonResponse)
{
    // First, parse the API response
    ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(jsonResponse);

    if (apiResponse == null || apiResponse.questions == null)
    {
        throw new Exception("Geçersiz veri formatı");
    }

    // Convert to our simplified format
    QuestionsInfo questionsInfo = new QuestionsInfo
    {
        questionType = DetermineQuestionType(apiResponse.question_type),
        questions = new List<Question>()
    };

    foreach (ApiQuestion apiQuestion in apiResponse.questions)
    {
        Question question = new Question
        {
            questionText = apiQuestion.question_text,
            hasImage = !string.IsNullOrEmpty(apiQuestion.image_path),
            imagePath = apiQuestion.image_path
        };

        // Process answers
        List<string> answers = new List<string>();
        int correctIndex = -1;

        for (int i = 0; i < apiQuestion.answers.Length; i++)
        {
            ApiAnswer answer = apiQuestion.answers[i];
            answers.Add(answer.answer_text);

            if (answer.is_correct)
            {
                correctIndex = i;
            }
        }

        question.answersText = answers.ToArray();
        question.correctAnswerId = correctIndex;

        questionsInfo.questions.Add(question);
    }

    return questionsInfo;
}

private void ClearOldData()
{
    // Clear all existing question data from PlayerPrefs

    // Clear main question data
    PlayerPrefs.DeleteKey(QUESTIONS_KEY);

    // Clear individual questions if they were split
    int questionsCount = PlayerPrefs.GetInt(QUESTIONS_COUNT_KEY, 0);
    for (int i = 0; i < questionsCount; i++)
    {
        string questionKey = QUESTIONS_KEY + "_" + i;
        PlayerPrefs.DeleteKey(questionKey);
    }

    // Clear image cache data
    foreach (Question question in allQuestions)
    {
        if (question.hasImage && !string.IsNullOrEmpty(question.imagePath))
        {
            string imageKey = "IMG_" + GetImageKeyFromPath(question.imagePath);
            PlayerPrefs.DeleteKey(imageKey);
        }
    }

    // Clear the memory cache as well
    imageCache.Clear();

    // Don't clear the LOGIN_KEY since the user is still logged in

    PlayerPrefs.Save();
}




    private IEnumerator FetchQuestions(string code)
    {
        string url = API_URL + code;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                loginPanel.GetComponent<LoginPanel>().ShowError("Hata: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                try
                {
                    ProcessApiResponse(jsonResponse);

                    // Mark as logged in
                    PlayerPrefs.SetInt(LOGIN_KEY, 1);
                    PlayerPrefs.Save();

                    // Hide login panel
                    loginPanel.SetActive(false);
                }
                catch (Exception e)
                {
                    loginPanel.GetComponent<LoginPanel>().ShowError("Veri işlenemedi: " + e.Message);
                    Debug.LogError("JSON Processing Error: " + e.Message);
                }
            }
        }
    }

    private void ProcessApiResponse(string jsonResponse)
    {
        // First, parse the API response
        ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(jsonResponse);

        if (apiResponse == null || apiResponse.questions == null)
        {
            throw new Exception("Geçersiz veri formatı");
        }

        // Convert to our simplified format
        QuestionsInfo questionsInfo = new QuestionsInfo
        {
            questionType = DetermineQuestionType(apiResponse.question_type),
            questions = new List<Question>()
        };

        foreach (ApiQuestion apiQuestion in apiResponse.questions)
        {
            Question question = new Question
            {
                questionText = apiQuestion.question_text,
                hasImage = !string.IsNullOrEmpty(apiQuestion.image_path),
                imagePath = apiQuestion.image_path
            };

            // Process answers
            List<string> answers = new List<string>();
            int correctIndex = -1;

            for (int i = 0; i < apiQuestion.answers.Length; i++)
            {
                ApiAnswer answer = apiQuestion.answers[i];
                answers.Add(answer.answer_text);

                if (answer.is_correct)
                {
                    correctIndex = i;
                }
            }

            question.answersText = answers.ToArray();
            question.correctAnswerId = correctIndex;

            questionsInfo.questions.Add(question);
        }

        // Save to PlayerPrefs
        SaveQuestionsToPlayerPrefs(questionsInfo);

        // Download and cache images
        StartCoroutine(DownloadAllImages(questionsInfo.questions));

        // Update our working copy
        allQuestions = questionsInfo.questions;
        GameSettings.TotalQuestionCount = allQuestions.Count;
    }
    private int DetermineQuestionType(string apiQuestionType)
    {
        // Map the API question type to your internal enum
        // Assuming: 0 = multiple_choice, 1 = true_false, etc.
        switch (apiQuestionType.ToLower())
        {
            case "multiple_choice":
                return 0;
            case "true_false":
                return 1;
            case "klasik":
                return 2;
            default:
                return 0; // Default to multiple choice
        }
    }
    private void SaveQuestionsToPlayerPrefs(QuestionsInfo questionsInfo)
    {
        // Store question type
        PlayerPrefs.SetInt(QUESTION_TYPE_KEY, questionsInfo.questionType);

        // Store questions count
        int questionsCount = questionsInfo.questions.Count;
        PlayerPrefs.SetInt(QUESTIONS_COUNT_KEY, questionsCount);

        // Convert all questions to JSON
        string jsonData = JsonUtility.ToJson(questionsInfo);

        // Check if we need to split the data due to size limits
        if (jsonData.Length < MAX_PREFS_SIZE)
        {
            // We can store everything in one key
            PlayerPrefs.SetString(QUESTIONS_KEY, jsonData);
        }
        else
        {
            // We need to split the data across multiple keys
            int chunkSize = MAX_PREFS_SIZE / 2; // Leave some margin

            for (int i = 0; i < questionsInfo.questions.Count; i++)
            {
                // Store each question individually
                string questionKey = QUESTIONS_KEY + "_" + i;
                string questionJson = JsonUtility.ToJson(questionsInfo.questions[i]);
                PlayerPrefs.SetString(questionKey, questionJson);
            }
        }

        PlayerPrefs.Save();
    }
      public void LoadQuestionsFromPlayerPrefs()
    {
        allQuestions.Clear();

        if (!PlayerPrefs.HasKey(QUESTIONS_COUNT_KEY))
        {
            Debug.LogWarning("No saved questions found!");
            return;
        }

        int questionType = PlayerPrefs.GetInt(QUESTION_TYPE_KEY, 0);
        int questionsCount = PlayerPrefs.GetInt(QUESTIONS_COUNT_KEY, 0);

        // Try to load all questions at once first
        if (PlayerPrefs.HasKey(QUESTIONS_KEY))
        {
            string jsonData = PlayerPrefs.GetString(QUESTIONS_KEY);
            QuestionsInfo questionsInfo = JsonUtility.FromJson<QuestionsInfo>(jsonData);

            if (questionsInfo != null && questionsInfo.questions != null)
            {
                allQuestions = questionsInfo.questions;

                // Start downloading images
                StartCoroutine(DownloadAllImages(allQuestions));
                return;
            }
        }

        // If that didn't work, try loading questions one by one
        for (int i = 0; i < questionsCount; i++)
        {
            string questionKey = QUESTIONS_KEY + "_" + i;

            if (PlayerPrefs.HasKey(questionKey))
            {
                string questionJson = PlayerPrefs.GetString(questionKey);
                Question question = JsonUtility.FromJson<Question>(questionJson);

                if (question != null)
                {
                    allQuestions.Add(question);
                }
            }
        }

        // Start downloading images
        StartCoroutine(DownloadAllImages(allQuestions));
    }
    private IEnumerator DownloadAllImages(List<Question> questions)
    {
        foreach (Question question in questions)
        {
            if (question.hasImage && !string.IsNullOrEmpty(question.imagePath))
            {
                // Check if we already have this image cached
                if (!imageCache.ContainsKey(question.imagePath))
                {
                    yield return StartCoroutine(DownloadAndCacheImage(question.imagePath));
                }
            }
        }

        Debug.Log("All images downloaded and cached.");
    }
    private IEnumerator DownloadAndCacheImage(string imagePath)
    {
        // Check if we already have this image in PlayerPrefs
        string imageKey = "IMG_" + GetImageKeyFromPath(imagePath);

        if (PlayerPrefs.HasKey(imageKey))
        {
            // Load from PlayerPrefs
            string base64Data = PlayerPrefs.GetString(imageKey);
            Texture2D texture = Base64ToTexture(base64Data);

            if (texture != null)
            {
                imageCache[imagePath] = texture;
                yield break;
            }
        }

        // Otherwise download it
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imagePath))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download image: " + request.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                imageCache[imagePath] = texture;

                // Convert to base64 and save to PlayerPrefs
                string base64Data = TextureToBase64(texture);
                PlayerPrefs.SetString(imageKey, base64Data);
                PlayerPrefs.Save();
            }
        }
    }
    private string GetImageKeyFromPath(string path)
    {
        // Create a unique but consistent key from the path
        // Using GetHashCode or a simple hash function is one approach
        return path.GetHashCode().ToString();
    }
    private string TextureToBase64(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        return Convert.ToBase64String(bytes);
    }
    private Texture2D Base64ToTexture(string base64)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(base64);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to convert base64 to texture: " + e.Message);
            return null;
        }
    }
    public Texture2D GetQuestionImage(Question question)
    {
        if (!question.hasImage || string.IsNullOrEmpty(question.imagePath))
        {
            return null;
        }

        if (imageCache.TryGetValue(question.imagePath, out Texture2D texture))
        {
            return texture;
        }

        // Image not yet downloaded, try to load from PlayerPrefs
        string imageKey = "IMG_" + GetImageKeyFromPath(question.imagePath);

        if (PlayerPrefs.HasKey(imageKey))
        {
            string base64Data = PlayerPrefs.GetString(imageKey);
            return Base64ToTexture(base64Data);
        }

        // Image not available yet
        return null;
    }



}