using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface ISongMessageTarget : IEventSystemHandler
{
    void Hit(int key);
}

public class Song : MonoBehaviour, ISongMessageTarget
{
    public GameObject[] keys;
    public GameObject wall;
    public int clipBeats;
    public double m_imprecisionTolerance = 0.25;

    double m_timeStart;
    double m_beatInterval;
    double m_timeNextResolution;
    List<int> m_beatKeys = new List<int>(new int[] { -1, -1, -1, -1 });
    int m_resolutions = 0;
    double m_timeNextBeat;
    int m_beats = 0;

	//Ajaskript
	public Text Endtext;
	public float EndScreenLength;
	public float EndScreenTime;
	//Ajaskript

    void Start()
    {
        GetComponent<AudioSource>().Play();
        m_timeStart = AudioSettings.dspTime;
        m_beatInterval = GetComponent<AudioSource>().clip.length / clipBeats;
        m_timeNextResolution = m_timeStart + (m_beatKeys.Count + m_imprecisionTolerance) * m_beatInterval;
        m_timeNextBeat = m_timeStart + m_beatInterval;

		//Ajaskript, cas objeveni endscreen = delka klipu  - doba trvani outra 
		//EndScreenTime = GetComponent<AudioSource>().clip.length - EndScreenLength;
		//Ajaskript
    }
    
    void Update()
    {
		//Ajaskript - ENDGAME
		EndScreenTime = GetComponent<AudioSource>().clip.length - EndScreenLength;
		Debug.Log(EndScreenTime);
		if (GetComponent<AudioSource>().time >= EndScreenTime)
		{
				//put anything related to endgame HERE
			Endtext.text = "you win";
		}
		else
		{
			Endtext.text = "playing";
			}
		//Ajaskript



        double time = AudioSettings.dspTime;
        if (time >= m_timeNextBeat)
        {
            m_timeNextBeat = m_timeNextBeat + m_beatInterval;
            ++m_beats;
            wall.GetComponent<TextMesh>().text = (m_beats % 4 + 1).ToString();
        }
        if (time >= m_timeNextResolution)
        {
            ++m_resolutions;
            m_timeNextResolution = m_timeStart + (m_resolutions * m_beatKeys.Count + m_imprecisionTolerance) * m_beatInterval;
            Debug.Log(string.Format("{0} {1} {2} {3}", m_beatKeys[0], m_beatKeys[1], m_beatKeys[2], m_beatKeys[3]));

            foreach (GameObject passerby in GameObject.FindGameObjectsWithTag("Passerby"))
            {
                ExecuteEvents.Execute<IPasserbyMessageTarget>(passerby, null, (x, y) => x.TrySequence(m_beatKeys));
            }

            m_beatKeys[0] = -1;
            m_beatKeys[1] = -1;
            m_beatKeys[2] = -1;
            m_beatKeys[3] = -1;
        }
    }

    public void Hit(int key)
    {
        double timeSinceStart = AudioSettings.dspTime - m_timeStart;
        int closestBeatIndex = (int)(timeSinceStart / m_beatInterval + 0.5);
        double closestBeatTimeSinceStart = m_beatInterval * closestBeatIndex;
        double timeFromClosestBeat = timeSinceStart - closestBeatTimeSinceStart;
        double imprecisionRatio = timeFromClosestBeat / m_beatInterval;
        bool correct = Math.Abs(imprecisionRatio) <= m_imprecisionTolerance;
        if (correct)
        {
            int beatIndexInSequence = closestBeatIndex % m_beatKeys.Count;
            if (m_beatKeys[beatIndexInSequence] == -1)
            {
                m_beatKeys[beatIndexInSequence] = key;
            }
            else
            {
                m_beatKeys[beatIndexInSequence] = -2;
                correct = false;
            }
        }
        double timeNextHalfBeat = m_timeStart + (closestBeatIndex + 0.5) * m_beatInterval;
        ExecuteEvents.Execute<IKeyMessageTarget>(keys[key], null, (x, y) => x.Hit(correct, timeNextHalfBeat));
    }
}
