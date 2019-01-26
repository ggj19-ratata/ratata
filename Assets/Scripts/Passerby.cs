﻿using System;
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
    public GameObject banner;
    public Musician musician;
    public List<int> sequence;
    public bool active = true;

    static char[] keyChars = { 'A', 'S', 'K', 'L' };

    void Start()
    {
        UpdateSequence();
    }

    public void TrySequence(List<int> playedSequence)
    {
        if (active && playedSequence.SequenceEqual(sequence))
        {
            GetComponent<AudioSource>().Play();
            musician.AddScore(1);
            active = false;
            banner.GetComponent<TextMesh>().text = "-";
        }
    }

    public void SetSequence(List<int> newSequence)
    {
        sequence = newSequence;
        UpdateSequence();
    }

    void UpdateSequence()
    {
        Debug.Log(String.Join(", ", sequence));
        Debug.Log(active);
        if (active)
        {
            Debug.Log(String.Join("", sequence.Select(i => keyChars[i])));
            banner.GetComponent<TextMesh>().text = String.Join("", sequence.Select(i => keyChars[i]));
        }
    }
}
