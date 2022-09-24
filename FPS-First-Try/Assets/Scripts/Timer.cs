using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Timer
    {
        public bool isTimerRunning;
        public float timeRemaining;
        void Update()
        {
            if (isTimerRunning)
            {
                if (timeRemaining > 0) timeRemaining -= Time.deltaTime;
                else
                {
                    timeRemaining = 0;
                    isTimerRunning = false;
                }
            }
        }
    public void StartTimer(float time)
    {
        isTimerRunning = true;
        timeRemaining = time;
    }

}

