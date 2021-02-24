// File     : Timer.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text TimeText;
    [SerializeField] private Text DayText;
    [SerializeField] private bool BoolTimer = false;
    private float StartTimer = 0;
    private float CurrentTimer;
    private float Tage = 0;
    private static float Stunden = 0;
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
        DayText.text = "Day: " + Tage;
        Stunden = test;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (BoolTimer)
        {
            CurrentTimer += 1.5f * Time.deltaTime;
            Minuten = (int)CurrentTimer;

            if (CurrentTimer >= 60)
            {
                Stunden++;

                Minuten = 0;
                CurrentTimer = StartTimer;
            }

            if (Stunden == 24)
            {
                Tage++;
                Stunden = 0;
            }
        }
    }
}
