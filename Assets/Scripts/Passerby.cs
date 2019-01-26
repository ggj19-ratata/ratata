using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Passerby : MonoBehaviour
{
    public List<int> sequence;

    static char[] keyChars = { 'A', 'S', 'K', 'L' };

    void Start()
    {
        GetComponent<TextMesh>().text = String.Join("", sequence.Select(i => keyChars[i]));
    }
}
