using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Keys keysObject;
    public GameObject[] keys;
    public GameObject beatCounter;
    public int clipBeats;
    public double m_imprecisionTolerance = 0.25;
    public double playbackDelay = 1.0; // Allows pre-loading the clip for better synchronization
    public int sequenceLength = 4;
    public int introBeats = 0;
    public AudioClip songMain;
    public AudioClip songExtra;
    public double halfBeatDistance = 0.25;

    double m_timeStart;
    double m_beatInterval;
    double m_timeNextResolution;
    Dictionary<int, BeatResult> beatResults = new Dictionary<int, BeatResult>();
    int m_resolutions = 0;
    double m_timeNextBeat;
    int m_beats = 0;
    AudioSource audioSourceMain;
    AudioSource audioSourceExtra;
    double timeNextEnable;
    double timeNextDisable;

    //Ajaskript
    public Text Endtext;
	public float EndScreenLength;
	public float EndScreenTime;
	public GameObject panel;
	public string ChangeToSceneName;
	//Ajaskript

    void Start()
    {
        m_timeStart = AudioSettings.dspTime + playbackDelay;
        audioSourceMain = gameObject.AddComponent<AudioSource>();
        audioSourceMain.clip = songMain;
        audioSourceMain.PlayScheduled(m_timeStart);
        audioSourceExtra = gameObject.AddComponent<AudioSource>();
        audioSourceExtra.clip = songExtra;
        audioSourceExtra.PlayScheduled(m_timeStart);
        audioSourceExtra.volume = 0f;
        m_beatInterval = GetComponent<AudioSource>().clip.length / clipBeats;
        m_timeNextResolution = m_timeStart + (sequenceLength + m_imprecisionTolerance) * m_beatInterval;
        m_timeNextBeat = m_timeStart + m_beatInterval;
        keysObject.SetEnabled(introBeats == 0);
        timeNextEnable = m_timeStart + (introBeats - 0.5) * m_beatInterval;
        timeNextDisable = m_timeStart + (introBeats + sequenceLength - 0.5) * m_beatInterval;

		//Ajaskript, cas objeveni endscreen = delka klipu  - doba trvani outra 
		EndScreenTime = GetComponent<AudioSource>().clip.length - EndScreenLength;
		//Ajaskript
    }
    
    void Update()
    {
		//Ajaskript - ENDGAME
		if (GetComponent<AudioSource>().time >= EndScreenTime)
		{
			//for testing the scene loading:
			//SceneManager.LoadScene(ChangeToSceneName);

			//put anything related to endgame HERE
			panel.SetActive(true);
			Endtext.text = "YOU WIN!\n\n\n*NAME* THE RAT HAS COLLECTED ENOUGH MONEY \nTO BUY HIMSELF A NEW THRASH CAN HOME! \n\n\nWELL DONE!";
		}
		else
		{
			Endtext.text = "";
		}

		//reload scene after song stops
		//if (GetComponent<AudioSource>().isPlaying == false)
		//{
		//	SceneManager.LoadScene(ChangeToSceneName);
			//panel.SetActive(false);
		//}
		//Ajaskript
        
        double time = AudioSettings.dspTime;
        if (time >= timeNextEnable)
        {
            keysObject.SetEnabled(true);
            timeNextEnable += sequenceLength * 2 * m_beatInterval;
        }
        if (time >= timeNextDisable)
        {
            keysObject.SetEnabled(false);
            timeNextDisable += sequenceLength * 2 * m_beatInterval;
            audioSourceExtra.volume = 0f;
        }
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
        double timeNextHalfBeat = m_timeStart + (closestBeatIndex + halfBeatDistance) * m_beatInterval;
        // TODO: Don't declare the beat correct if it coincides with another beat.
        ExecuteEvents.Execute<IKeyMessageTarget>(keys[key], null, (x, y) => x.Hit(correct, timeNextHalfBeat));
    }

    public void RegisterSuccess()
    {
        audioSourceExtra.volume = 1f;
    }
}
