using UnityEngine;
using UnityEngine.EventSystems;

public interface IKeyMessageTarget : IEventSystemHandler
{
    void Hit(bool correct, int beatIndex);
    void HalfBeat(int beatIndex);
}

public class Key : MonoBehaviour, IKeyMessageTarget
{
    public AudioClip successImmediate;
    public AudioClip successAfter;
    public AudioClip failure;

    SpriteRenderer m_SpriteRenderer;
    AudioSource audioSource;
    int lastBeatIndex = -1;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hit(bool correct, int beatIndex)
    {
        if (correct)
        {
            m_SpriteRenderer.color = Random.ColorHSV();
            audioSource.PlayOneShot(successImmediate);
            lastBeatIndex = beatIndex;
        }
        else
        {
            audioSource.PlayOneShot(failure);
            lastBeatIndex = -1;
        }
    }

    public void HalfBeat(int beatIndex)
    {
        if (beatIndex == lastBeatIndex)
        {
            audioSource.PlayOneShot(successAfter);
        }
    }
}
