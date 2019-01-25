using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{
    float m_timeStart;
    float m_beatInterval = 60.0f / 142;
    float m_timeNextBeat;

    // Start is called before the first frame update
    void Start()
    {
        m_timeStart = Time.time;
        UpdateTimeNextBeat();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= m_timeNextBeat)
        {
            Debug.Log("beat");
            UpdateTimeNextBeat();
        }
    }

    void UpdateTimeNextBeat()
    {
        m_timeNextBeat = Time.time + m_beatInterval;
    }
}
