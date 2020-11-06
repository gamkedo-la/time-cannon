using UnityEngine;

public static class TimeCannonPlayerPrefs
{
    public static void SetTimePeriod(TimePeriod timePeriod)
    {
        PlayerPrefs.SetInt("stageNow", (int)timePeriod);
    }
}
