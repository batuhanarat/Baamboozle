public static class GameSettings
{
    public static int CardSize;
    public static bool isCardSizeChangedFromSettings;
    public static int TeamSize;
    public static bool isTeamSizeChangedFromSettings;
    public static bool isTeamNamesChangedFromSettings;
    public static string[] TeamNames;
    public static bool isOptionCountsChangedFromSettings;
    public static int OptionCounts;
    public static bool isSpecialArrayChangedFromSettings;
    public static int[] SpecialArray;
    public static bool isTimeSettingsChangedFromSettings;
    public static TimeSetting TimeSetting;
    public static bool SpecialBlockAllowed;
    public static int TotalQuestionCount;



    public static int GetCardSize()
    {
        if(isCardSizeChangedFromSettings)
        {
            return CardSize;
        }

        if(isTeamSizeChangedFromSettings)
        {
            var teamsize = GetTeamSize();
            if(teamsize == 1 || teamsize == 2 || teamsize == 4 || teamsize == 8 )
            {
                return 16;
            }
            else if ( teamsize == 3)
            {
                return 15;
            }
            else if(teamsize == 5 ) {
                return 20;
            }
            else if(teamsize == 6)
            {
                return 18;
            }
            else if(teamsize == 7) {
                return 14;
            }
        }

        return 16;

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

    public static string[] GetTeamNames()
    {
        if(isTeamNamesChangedFromSettings)
        {
            return TeamNames;
        }
        else return null;
    }

    public static int GetTotalQuestionCount()
    {
        return TotalQuestionCount;

    }




}