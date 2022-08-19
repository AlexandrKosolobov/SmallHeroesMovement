using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private static int DAY_PER_WEEK = 7;
    [SerializeField] private static int HOURS_PER_DAY = 24;
    [SerializeField] private static int MINUSTES_PER_HOUR = 1;
    public int HOUR_LENGTH { get => MINUSTES_PER_HOUR; }
    public int DAY_LENGTH { get => HOUR_LENGTH * HOURS_PER_DAY; }
    public int WEEK_LENGTH { get => DAY_LENGTH * DAY_PER_WEEK; }

    private int lastHour = 0;
    private void Update()
    {
        if (lastHour != GetHoursOfDay())
        {
            lastHour = GetHoursOfDay();
            Debug.Log(lastHour);
        }
    }

    /// <summary>
    /// 0 -- 59 minutes
    /// </summary>
    /// <returns></returns>
    public int GetMinutesOfHour()
    {
        return (int)Time.time % HOUR_LENGTH;
    }

    /// <summary>
    /// 0 -- 23 hours
    /// </summary>
    /// <returns></returns>
    public int GetHoursOfDay()
    {
        return (int)(Time.time % DAY_LENGTH) / HOUR_LENGTH;
    }

    /// <summary>
    /// 0 -- 6 days
    /// </summary>
    /// <returns></returns>
    public int GetDayOfWeek()
    {
        return (int)(Time.time % WEEK_LENGTH) / DAY_LENGTH;
    }

    /// <summary>
    /// 0 -- +inf days
    /// </summary>
    /// <returns></returns>
    public int GetDays()
    {
        return (int)Time.time / DAY_LENGTH;
    }
}
