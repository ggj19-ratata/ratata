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
    
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.blue;
    }

    public void Hit(bool correct)
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (correct)
        {
            audio.clip = success;
            m_SpriteRenderer.color = Random.ColorHSV();
        }
        else
        {
            audio.clip = failure;
        }
        audio.Play();
    }
}
