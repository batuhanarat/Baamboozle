using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;


public class ApiManager
{

    private static ApiManager instance = null;
    public static ApiManager Instance {
        get {
        instance ??= new ApiManager();
        return instance;
        }
    }
    private ApiManager()
    {
    }
        private const string QUESTIONS_PREFIX = "questions_";

    public async Task<QuestionsInfo> GetQuestions(string code, string gameType, Action callbackSuccess, Action callbackFail)
    {
        string cacheKey = $"{QUESTIONS_PREFIX}{code}_{gameType}";

        QuestionsInfo cachedData = await LoadQuestionsFromCache(cacheKey);
        if (cachedData != null)
        {
            Debug.Log($"Önbellekten yüklendi: {cacheKey}");
            callbackSuccess?.Invoke();
            return cachedData;
        }

        // Veri bulunamadıysa API'den al
        string url = $"{API_URL}/questions";
        var parameters = new List<string>();
        if (!string.IsNullOrEmpty(code)) parameters.Add($"Code={code}");
        if (!string.IsNullOrEmpty(gameType)) parameters.Add($"GameType={gameType}");
        if (parameters.Count > 0)
        {
            url += "?" + string.Join("&", parameters);
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            try
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Error: {request.error}");
                    callbackFail?.Invoke();
                    return null;
                }

                string jsonResponse = request.downloadHandler.text;

                // JSON'ı QuestionsInfo nesnesine dönüştür
                QuestionsInfo response = JsonUtility.FromJson<QuestionsInfo>(jsonResponse);

                // Yerel depolamaya kaydet
                await SaveQuestionsToCache(cacheKey, jsonResponse);

                Debug.Log($"API'den alındı ve kaydedildi: {cacheKey}");
                callbackSuccess?.Invoke();
                return response;
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {e.Message}");
                callbackFail?.Invoke();
                return null;
            }
        }
    }

    // JSON verisini yerel depolamaya kaydetme
    private async Task SaveQuestionsToCache(string key, string jsonData)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, key);
            File.WriteAllText(filePath, jsonData);
            await Task.CompletedTask; // WebGL için async tutarlılığı
        }
        catch (Exception e)
        {
            Debug.LogError($"Veri kaydedilemedi: {e.Message}");
        }
    }

    // JSON verisini yerel depolamadan yükleme
    private async Task<QuestionsInfo> LoadQuestionsFromCache(string key)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, key);

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                QuestionsInfo cachedQuestions = JsonUtility.FromJson<QuestionsInfo>(jsonData);
                await Task.CompletedTask; // WebGL için async tutarlılığı
                return cachedQuestions;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Önbellek okunamadı: {e.Message}");
        }

        return null;
    }

    // Önbellekteki tüm soruları silme (opsiyonel)
    public void ClearQuestionsCache()
    {
        try
        {
            string directory = Application.persistentDataPath;
            string[] files = Directory.GetFiles(directory);

            foreach (string file in files)
            {
                if (Path.GetFileName(file).StartsWith(QUESTIONS_PREFIX))
                {
                    File.Delete(file);
                    Debug.Log($"Silindi: {file}");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Önbellek temizlenemedi: {e.Message}");
        }
    }


    public async Task DownloadMediaDirectly(string code)
    {
        string baseUrl = "https://www.ingilizcelik.com/wp-json/game/v1";
        string videoUrl = $"{baseUrl}/media/video/{code}";
        string imageUrl = $"{baseUrl}/media/image/{code}";

        using (UnityWebRequest videoRequest = UnityWebRequest.Get(videoUrl))
        {
            await videoRequest.SendWebRequest();
            if (videoRequest.result == UnityWebRequest.Result.Success)
            {
                byte[] videoData = videoRequest.downloadHandler.data;
                string videoPath = Path.Combine(Application.persistentDataPath, $"{code}.mp4");
                File.WriteAllBytes(videoPath, videoData);
            }
        }


        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            await imageRequest.SendWebRequest();
            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                // Texture olarak al
                Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
                // İstersen PNG olarak kaydet
                byte[] pngData = texture.EncodeToPNG();
                string imagePath = Path.Combine(Application.persistentDataPath, $"{code}.png");
                File.WriteAllBytes(imagePath, pngData);
            }
        }
    }
        private const string API_URL = "https://www.ingilizcelik.com/wp-json/game/v1";

    public async Task<QuestionsInfo> GetAds(string code, Action callbackSuccess, Action callbackFail)
    {

        string url = $"{API_URL}/ads";
        var parameters = new System.Collections.Generic.List<string>();


        if (!string.IsNullOrEmpty(code)) parameters.Add($"Code={code}");

        if (parameters.Count > 0)
        {
            url += "?" + string.Join("&", parameters);
        }

        /*
            if (!string.IsNullOrEmpty(code)) request.SetRequestHeader("Code", code);
            if (!string.IsNullOrEmpty(gameType)) request.SetRequestHeader("GameType", gameType);

        */

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            try
            {
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log($"Error: {request.error}");
                    callbackFail();
                    return null;
                }

                string jsonResponse = request.downloadHandler.text;
                QuestionsInfo response = JsonUtility.FromJson<QuestionsInfo>(jsonResponse);
                callbackSuccess();
                return response;
            }
            catch (Exception e)
            {
                Debug.Log($"Exception: {e.Message}");
                callbackFail();
                return null;
            }
        }
    }
}