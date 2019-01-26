using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IPasserbyMessageTarget : IEventSystemHandler
{
    void TrySequence(List<int> playedSequence);
}

public class Passerby : MonoBehaviour, IPasserbyMessageTarget
{
    public List<int> sequence;
    public Musician musician;

    static char[] keyChars = { 'A', 'S', 'K', 'L' };

    void Start()
    {
        GetComponent<TextMesh>().text = String.Join("", sequence.Select(i => keyChars[i]));
    }

    public void TrySequence(List<int> playedSequence)
    {
        if (playedSequence.SequenceEqual(sequence))
        {
            GetComponent<AudioSource>().Play();
            musician.AddScore(1);
        }
    }
}
