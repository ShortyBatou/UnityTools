using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static bool scaleTime;
    private static List<TimeLine> timeLines;

    private class TimeLine
    {
        public float duration;
        public float scale;
        public TimeLine(float duration, float scale)
        {
            this.duration = duration;
            this.scale = scale;
        }
        public void MinusTime(float minus)
        {
            this.duration -= minus;
        }

    }
    void Start()
    {
        timeLines = new List<TimeLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scaleTime)
        {

            int length = timeLines.Count;
            if (length == 0)
            {
                scaleTime = false;
                transform.localPosition = new Vector3(0, 0, 0);
                Time.timeScale = 1;
            }
            else
            {
                float scale = 0;
                for (int i = 0; i < length; i++)
                {
                    timeLines[i].MinusTime(Time.fixedDeltaTime);

                    if (timeLines[i].duration <= 0)
                    {
                        timeLines.Remove(timeLines[i]);
                        i--;
                        length--;
                    }
                    else
                    {

                        scale = Mathf.Max(timeLines[i].scale, scale);
                    }

                }
                Time.timeScale = scale;
            }
        }

    }

    public static void ChangeTimeScale(float scale,float duration)
    {
        timeLines.Add(new TimeLine(duration, scale));
        scaleTime = true;
    }
}
