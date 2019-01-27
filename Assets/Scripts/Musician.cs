using UnityEngine;

public class Musician : MonoBehaviour
{
    public int score = 0;
    public MoneyCounter moneyCounter;

    public void AddScore(int scoreDiff)
    {
        score += scoreDiff;
        moneyCounter.SetCount(this.score);
    }
}
