using UnityEngine;
using UnityEngine.EventSystems;

public interface IKeyMessageTarget : IEventSystemHandler
{
    void Hit(bool correct, double timeNextHalfBeat);
}

public class Key : MonoBehaviour, IKeyMessageTarget
{
    public AudioClip successImmediate;
    public AudioClip successAfter;
    public AudioClip failure;

    SpriteRenderer m_SpriteRenderer;
    AudioSource audioSourceImmediate;
    AudioSource audioSourceAfter;
    
    void Start()
    {
        audioSourceImmediate = gameObject.AddComponent<AudioSource>();
        audioSourceAfter = gameObject.AddComponent<AudioSource>();
        audioSourceAfter.clip = successAfter;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hit(bool correct, double timeNextHalfBeat)
    {
        if (correct)
        {
            m_SpriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            audioSourceImmediate.PlayOneShot(successImmediate);
            audioSourceAfter.PlayScheduled(timeNextHalfBeat);
        }
        else
        {
            audioSourceImmediate.PlayOneShot(failure);
        }
    }
}
