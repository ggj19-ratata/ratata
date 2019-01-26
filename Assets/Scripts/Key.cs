using UnityEngine;
using UnityEngine.EventSystems;

public interface IKeyMessageTarget : IEventSystemHandler
{
    void Hit(bool correct);
}

public class Key : MonoBehaviour, IKeyMessageTarget
{
    SpriteRenderer m_SpriteRenderer;
    
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.blue;
    }

    public void Hit(bool correct)
    {
        m_SpriteRenderer.color = Random.ColorHSV();
    }
}
