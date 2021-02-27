// File     : Timer.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // -------------------------------------------------------------------------------------------------------------

    #region Init

    //Events -------------------------------------------------------------------------------------------------------

    public static event Action<int> OnDayChanged;
    
    //Serialize Fields ---------------------------------------------------------------------------------------------

    [SerializeField] private Text TimeText;
    [SerializeField] private Text DayText;
    [SerializeField] private bool BoolTimer = false;
    [SerializeField] private float TimeSpeed = 1.5f;

    // Static Variables ---------------------------------------------------------------------------------------------

    private static int Day = 0;
    private static int Stunden = 0;
    private static float Minuten = 0;

    // Get properties ----------------------------------------------------------------------------------------------

    public static int GetDay => Day;
    public static float GetStunden => Stunden;
    public static float GetMinuten => Minuten;

    // Handle Properties --------------------------------------------------------------------------------------------

    private int DayH
    {
        get => Day;
        set
        {
            if (value != Day)
            {
                Day = value;
                OnDayChanged?.Invoke(Day);
            }
        }
    }

    // Private -------------------------------------------------------------------------------------------------------

    private float StartTimer = 0.01f;
    private float CurrentTimer;

    #endregion

    // -------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        StartTimer = CurrentTimer;
    }

    void Update()
    {
        DisplayTime(StartTimer);
        TimeText.text = string.Format("{0:00}:{1:00}", Stunden, Minuten);
        DayText.text = "Day: " + DayH;
    }

    /// <summary>
    /// Calculate Time to Display it
    /// </summary>
    /// <param name="timeToDisplay">Timer Variable</param>
    void DisplayTime(float timeToDisplay)
    {
        if (BoolTimer)
        {
            CurrentTimer += TimeSpeed * Time.deltaTime;
            Minuten = (int)CurrentTimer;

            if (CurrentTimer >= 60)
            {
                
                Stunden++;

                Minuten = 0;
                CurrentTimer = StartTimer;
            }

            if (Stunden == 24)
            {
                DayH++;
                Stunden = 0;
            }
        }
    }
}
