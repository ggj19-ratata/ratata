using System;
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
    public string button;
    public Song song;

    SpriteRenderer m_SpriteRenderer;
    AudioSource audioSourceImmediate;
    AudioSource audioSourceAfter;
    bool keyEnabled;
    
    void Start()
    {
        audioSourceImmediate = gameObject.AddComponent<AudioSource>();
        audioSourceAfter = gameObject.AddComponent<AudioSource>();
        audioSourceAfter.clip = successAfter;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // TODO: Decrease lag. Consider using FixedUpdate.
        if (keyEnabled && !String.IsNullOrEmpty(button) && Input.GetButtonDown(button))
        {
            song.Hit(transform.GetSiblingIndex());
        }
    }

    public void Hit(bool correct, double timeNextHalfBeat)
    {
        if (correct)
        {
            m_SpriteRenderer.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            audioSourceImmediate.PlayOneShot(successImmediate);
            audioSourceAfter.PlayScheduled(timeNextHalfBeat);
            GetComponent<Animator>().SetTrigger("Bounce");
        }
        else
        {
            audioSourceImmediate.PlayOneShot(failure);
        }
    }

    public void SetEnabled(bool active)
    {
        keyEnabled = active;
        var color = m_SpriteRenderer.color;
        if (active)
        {
            color.a = 1f;
        }
        else
        {
            color.a = 0.25f;
        }
        m_SpriteRenderer.color = color;
    }
}
