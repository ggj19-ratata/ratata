using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Musician : MonoBehaviour
{
    public string[] buttons;
    public GameObject song;
    public int score = 0;
    public MoneyCounter moneyCounter;
    
    void Update()
    {
        // TODO: Decrease lag. Consider using FixedUpdate.
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

    public void AddScore(int scoreDiff)
    {
        score += scoreDiff;
        moneyCounter.SetCount(this.score);
    }
}
