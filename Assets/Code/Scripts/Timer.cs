// File     : Timer.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public static event Action<int> OnDayChanged; 
    [SerializeField] private Text TimeText;
    [SerializeField] private Text DayText;
    [SerializeField] private bool BoolTimer = false;
    private float StartTimer = 0.01f;
    private float CurrentTimer;
    [SerializeField] private float TimeSpeed = 1.5f;

    private int Day = 0;

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
    private static int Stunden = 0;
    private static float Minuten = 0;
    public float test;

    public static float GetStunden => Stunden;
    public static float GetMinuten => Minuten;

    private void Start()
    {
        StartTimer = CurrentTimer;
    }

    void Update()
    {
        DisplayTime(StartTimer);
        TimeText.text = string.Format("{0:00}:{1:00}", Stunden, Minuten);
        DayText.text = "Day: " + DayH;
        //Stunden = test;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (BoolTimer)
        {
            CurrentTimer += TimeSpeed * Time.deltaTime;
            Minuten = (int)CurrentTimer;

            if (CurrentTimer >= 60)
            {
                Debug.Log("stunden" + Stunden);
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
