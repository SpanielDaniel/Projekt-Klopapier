// File     : Timer.cs
// Author   : Daniel Pobijanski
// Project  : Projekt-Klopapier

using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text TimeText;
    private float StartTimer = 0;

    void Update()
    {
        StartTimer += Time.deltaTime;

        DisplayTime(StartTimer);
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
