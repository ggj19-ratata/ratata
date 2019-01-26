using UnityEngine;
using UnityEngine.EventSystems;

public interface IKeyMessageTarget : IEventSystemHandler
{
    void Hit(bool correct);
}

public class Key : MonoBehaviour, IKeyMessageTarget
{
    public AudioClip success;
    public AudioClip failure;

    SpriteRenderer m_SpriteRenderer;
    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hit(bool correct)
    {
        if (correct)
        {
            m_SpriteRenderer.color = Random.ColorHSV();
            audioSource.PlayOneShot(success);
        }
        else
        {
            audioSource.PlayOneShot(failure);
        }
    }
}
