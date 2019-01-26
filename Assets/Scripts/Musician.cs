using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Musician : MonoBehaviour
{
    public string[] buttons;
    public GameObject song;
    
    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            string button = buttons[i];
            if (String.IsNullOrEmpty(button))
            {
                continue;
            }
            if (Input.GetButtonDown(button))
            {
                ExecuteEvents.Execute<ISongMessageTarget>(song, null, (x, y) => x.Hit(i));
            }
        }
    }
}
