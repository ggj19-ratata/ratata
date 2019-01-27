using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    public void SetEnabled(bool active)
    {
        foreach (Key key in GetComponentsInChildren<Key>())
        {
            key.SetEnabled(active);
        }
    }
}
