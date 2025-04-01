using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, IProvidable> _registerDictionary = new();
    public static TeamManager TeamManager => GetManager<TeamManager>();
    public static SpecialItemFactory SpecialItemFactory => GetManager<SpecialItemFactory>();
    public static AssetLibrary AssetLibrary => GetManager<AssetLibrary>();
    public static QuestionManager QuestionManager => GetManager<QuestionManager>();
    public static TeamSizeSelector TeamSizeSelector => GetManager<TeamSizeSelector>();
    public static CardSizeSelector CardSizeSelector => GetManager<CardSizeSelector>();
    public static TimeSettingSelector TimeSettingSelector => GetManager<TimeSettingSelector>();
    public static OptionCountSelectorForMultipleChoiceQuestion OptionCountSelectorForMultipleChoiceQuestion => GetManager<OptionCountSelectorForMultipleChoiceQuestion>();
    public static SpecialItemSelector SpecialItemSelector => GetManager<SpecialItemSelector>();
    public static ScoreManager ScoreManager => GetManager<ScoreManager>();



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {
        InitializeCoreServices();
    }

    private static void InitializeCoreServices()
    {
        _ = new QuestionManager();
        _ = new TeamManager();
        _ = new SpecialItemFactory();
    }

    private static void RegisterSceneEvents()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    private static T GetManager<T>() where T : class, IProvidable
    {
        if (_registerDictionary.ContainsKey(typeof(T)))
        {
            return (T)_registerDictionary[typeof(T)];
        }

        return null;
    }

    public static T Register<T>(T target) where T: class, IProvidable
    {
        _registerDictionary[typeof(T)] = target;
        return target;
    }
}