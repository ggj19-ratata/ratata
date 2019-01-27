using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IKeyMessageTarget : IEventSystemHandler
{
    void Hit(bool correct, double beatTime, double beatDuration);
}

public class Key : MonoBehaviour, IKeyMessageTarget
{
    public AudioClip successImmediate;
    public AudioClip successAfter;
    public AudioClip failure;
    public string button;
    public Song song;
    public double afterInterval = 0.25; // in beats

    AudioSource audioSourceImmediate;
    AudioSource audioSourceAfter;
    bool keyEnabled;
    
    void Start()
    {
        audioSourceImmediate = gameObject.AddComponent<AudioSource>();
        audioSourceAfter = gameObject.AddComponent<AudioSource>();
        audioSourceAfter.clip = successAfter;
    }

    void Update()
    {
        // TODO: Decrease lag. Consider using FixedUpdate.
        if (keyEnabled && !String.IsNullOrEmpty(button) && Input.GetButtonDown(button))
        {
            song.Hit(transform.GetSiblingIndex());
        }
    }

    public void Hit(bool correct, double beatTime, double beatDuration)
    {
        if (correct)
        {
            GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            audioSourceImmediate.PlayOneShot(successImmediate);
            audioSourceAfter.PlayScheduled(beatTime + afterInterval * beatDuration);
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
        var color = GetComponent<SpriteRenderer>().color;
        if (active)
        {
            color.a = 1f;
        }
        else
        {
            color.a = 0.25f;
        }
        GetComponent<SpriteRenderer>().color = color;
    }
}
