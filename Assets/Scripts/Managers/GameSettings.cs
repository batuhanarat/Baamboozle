public static class GameSettings
{
    public static int CardSize;
    public static bool isCardSizeChangedFromSettings;
    public static int TeamSize;
    public static bool isTeamSizeChangedFromSettings;
    public static string[] TeamNames;
    public static bool isOptionCountsChangedFromSettings;
    public static int OptionCounts;
    public static bool isSpecialArrayChangedFromSettings;
    public static int[] SpecialArray;
    public static bool isTimeSettingsChangedFromSettings;
    public static TimeSetting TimeSetting;

    public static int GetCardSize()
    {
        if(isCardSizeChangedFromSettings)
        {
            return CardSize;
        }
        else return 16;
    }

    public static int GetTeamSize()
    {
        if(isTeamSizeChangedFromSettings)
        {
            return TeamSize;
        }
        else return 4;
    }

    public static int GetOptionCounts()
    {
        if(isOptionCountsChangedFromSettings)
        {
            return OptionCounts;
        }
        else return 4;
    }

    public static int[] GetSpecialArray()
    {
        if(isSpecialArrayChangedFromSettings)
        {
            return SpecialArray;
        }
        else
        {
            int[] specialArray = new int[11];
            for(int i = 0 ; i< specialArray.Length ; i++)
            {
                specialArray[i] = 1;
            }
            return specialArray;
        }
    }
    public static TimeSetting GetTimeSetting()
    {
        if(isTimeSettingsChangedFromSettings)
        {
            return TimeSetting;
        }
        else return TimeSetting.DECREASING;
    }


}