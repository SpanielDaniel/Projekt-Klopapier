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
    public float StartTimer = 0;
    public float CurrentTimer;
    public float Tage = 0;
    public float Stunden = 0;
    public float Minuten = 0;

    private void Start()
    {
        StartTimer = CurrentTimer;
    }

    void Update()
    {
        DisplayTime(StartTimer);
        TimeText.text = string.Format("{0:00}:{1:00}", Stunden, Minuten);
        DayText.text = "Day: " + Tage;
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
