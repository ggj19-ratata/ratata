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
    float m_timeNextBeat;
    int beatIndex = 0;

    public void Hit(int key)
    {
        float timeSinceStart = Time.time - m_timeStart;
        float timeFromClosestBeat = (timeSinceStart + m_beatInterval/2) % m_beatInterval - m_beatInterval/2;
        float imprecisionRatio = timeFromClosestBeat*2 / m_beatInterval;
        Debug.Log(imprecisionRatio);
        bool correct = Mathf.Abs(imprecisionRatio) <= 0.25;
        ExecuteEvents.Execute<IKeyMessageTarget>(keys[key], null, (x, y) => x.Hit(correct));
    }

    void Start()
    {
        m_timeStart = Time.time;
        UpdateTimeNextBeat();
    }
    
    void Update()
    {
        if (Time.time >= m_timeNextBeat)
        {
            //Debug.Log("beat");
            ++beatIndex;
            UpdateTimeNextBeat();
        }
    }

    void UpdateTimeNextBeat()
    {
        m_timeNextBeat = Time.time + m_beatInterval;
    }
}
