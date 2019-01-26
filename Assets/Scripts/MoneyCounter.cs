using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    void Start()
    {
        SetCount(0);
    }

    public void SetCount(int count)
    {
        GetComponent<TextMesh>().text = string.Format("Money: {0}", count);
    }
}
