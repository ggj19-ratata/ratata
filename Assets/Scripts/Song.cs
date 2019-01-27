﻿using System;
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
    private class BeatResult
    {
        public HashSet<int> hits = new HashSet<int>();
        public HashSet<int> misses = new HashSet<int>();
    }

    public GameObject[] keys;
    public GameObject beatCounter;
    public int clipBeats;
    public double m_imprecisionTolerance = 0.25;
    public double playbackDelay = 1.0; // Allows pre-loading the clip for better synchronization
    public int sequenceLength = 4;

    double m_timeStart;
    double m_beatInterval;
    double m_timeNextResolution;
    Dictionary<int, BeatResult> beatResults = new Dictionary<int, BeatResult>();
    int m_resolutions = 0;
    double m_timeNextBeat;
    int m_beats = 0;

	//Ajaskript
	public Text Endtext;
	public float EndScreenLength;
	public float EndScreenTime;
	public GameObject panel;
	//Ajaskript

    void Start()
    {
        m_timeStart = AudioSettings.dspTime + playbackDelay;
        GetComponent<AudioSource>().PlayScheduled(m_timeStart);
        m_beatInterval = GetComponent<AudioSource>().clip.length / clipBeats;
        m_timeNextResolution = m_timeStart + (sequenceLength + m_imprecisionTolerance) * m_beatInterval;
        m_timeNextBeat = m_timeStart + m_beatInterval;

		//Ajaskript, cas objeveni endscreen = delka klipu  - doba trvani outra 
		EndScreenTime = GetComponent<AudioSource>().clip.length - EndScreenLength;
		//Ajaskript
    }
    
    void Update()
    {
		//Ajaskript - ENDGAME
		if (GetComponent<AudioSource>().time >= EndScreenTime)
		{
			//put anything related to endgame HERE
			panel.SetActive(true);
			Endtext.text = "YOU WIN! \n*NAME* THE RAT HAS COLLECTED ENOUGH MONEY TO BUY HIMSELF A NEW THRASH CAN HOME! \nWELL DONE!";
		}
		else
		{
			Endtext.text = "";
		}

		//Ajaskript
        
        double time = AudioSettings.dspTime;
        if (time >= m_timeNextBeat)
        {
            m_timeNextBeat += m_beatInterval;
            ++m_beats;
            beatCounter.GetComponent<TextMesh>().text = (m_beats % 4 + 1).ToString();
        }
        if (time >= m_timeNextResolution)
        {
            List<int> sequence = new List<int>();
            for (int i = 0; i < sequenceLength; ++i)
            {
                int beatIndex = m_beats - sequenceLength + i;
                int seqElem = -1;
                if (beatResults.ContainsKey(beatIndex))
                {
                    var beatResult = beatResults[beatIndex];
                    if (beatResult.misses.Count > 0 || beatResult.hits.Count > 1)
                    {
                        seqElem = -2;
                        continue;
                    }
                    if (beatResult.hits.Count == 1)
                    {
                        seqElem = beatResult.hits.First();
                    }
                    beatResults.Remove(beatIndex);
                }
                sequence.Add(seqElem);
            }
            Debug.Log(String.Join(" ", sequence));
            foreach (GameObject passerby in GameObject.FindGameObjectsWithTag("Passerby"))
            {
                ExecuteEvents.Execute<IPasserbyMessageTarget>(passerby, null, (x, y) => x.TrySequence(sequence));
            }

            ++m_resolutions;
            m_timeNextResolution = m_timeStart + ((m_resolutions+1) * sequenceLength + m_imprecisionTolerance) * m_beatInterval;
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
        if (!beatResults.ContainsKey(closestBeatIndex))
        {
            beatResults[closestBeatIndex] = new BeatResult();
        }
        if (correct)
        {
            beatResults[closestBeatIndex].hits.Add(key);
        }
        else
        {
            beatResults[closestBeatIndex].misses.Add(key);
        }
        double timeNextHalfBeat = m_timeStart + (closestBeatIndex + 0.5) * m_beatInterval;
        ExecuteEvents.Execute<IKeyMessageTarget>(keys[key], null, (x, y) => x.Hit(correct, timeNextHalfBeat));
    }
}
