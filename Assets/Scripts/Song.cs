using UnityEngine;
using UnityEngine.EventSystems;

public interface ISongMessageTarget : IEventSystemHandler
{
    void Hit(int key);
}

public class Song : MonoBehaviour, ISongMessageTarget
{
    public GameObject[] keys;

    float m_timeStart;
    float m_beatInterval = 60.0f / 142;
    float m_imprecisionTolerance = 0.25f;
    float m_timeNextResolution;
    int[] m_beatKeys = { -1, -1, -1, -1 };
    int m_resolutions = 0;

    public void Hit(int key)
    {
        float timeSinceStart = Time.time - m_timeStart;
        int closestBeatIndex = (int)(timeSinceStart / m_beatInterval + 0.5);
        float closestBeatTimeSinceStart = m_beatInterval * closestBeatIndex;
        float timeFromClosestBeat = timeSinceStart - closestBeatTimeSinceStart;
        float imprecisionRatio = timeFromClosestBeat / m_beatInterval;
        Debug.Log(imprecisionRatio);
        bool correct = Mathf.Abs(imprecisionRatio) <= m_imprecisionTolerance;
        if (correct)
        {
            int beatIndexInSequence = closestBeatIndex % m_beatKeys.Length;
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
        ExecuteEvents.Execute<IKeyMessageTarget>(keys[key], null, (x, y) => x.Hit(correct));
    }

    void Start()
    {
        GetComponent<AudioSource>().Play();
        m_timeStart = Time.time;
        m_timeNextResolution = m_timeStart + (m_beatKeys.Length + m_imprecisionTolerance) * m_beatInterval;
    }
    
    void Update()
    {
        if (Time.time >= m_timeNextResolution)
        {
            ++m_resolutions;
            m_timeNextResolution = m_timeStart + (m_resolutions * m_beatKeys.Length + m_imprecisionTolerance) * m_beatInterval;
            Debug.Log(string.Format("{0} {1} {2} {3}", m_beatKeys[0], m_beatKeys[1], m_beatKeys[2], m_beatKeys[3]));
            m_beatKeys[0] = -1;
            m_beatKeys[1] = -1;
            m_beatKeys[2] = -1;
            m_beatKeys[3] = -1;
        }
    }
}
