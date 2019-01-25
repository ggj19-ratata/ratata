using UnityEngine;

public class Key : MonoBehaviour, IHitMessageTarget
{
    SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the SpriteRenderer from the GameObject
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        //Set the GameObject's Color quickly to a set Color (blue)
        m_SpriteRenderer.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        m_SpriteRenderer.color = Color.blue;
    }

    void IHitMessageTarget.Hit()
    {
        m_SpriteRenderer.color = Color.red;
    }
}
