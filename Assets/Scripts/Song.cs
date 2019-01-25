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

    public void Hit(int key)
    {
        // Calculate precision
        ExecuteEvents.Execute<IKeyMessageTarget>(keys[key], null, (x, y) => x.Hit(true));
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
            Debug.Log("beat");
            UpdateTimeNextBeat();
        }
    }

    void UpdateTimeNextBeat()
    {
        m_timeNextBeat = Time.time + m_beatInterval;
    }
}
