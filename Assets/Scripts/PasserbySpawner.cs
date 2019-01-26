using System;
using System.Collections.Generic;
using UnityEngine;

public class PasserbySpawner : MonoBehaviour
{
    public Musician musician;
    public Passerby passerbyPrefab;
    public float radius = 1f;
    public float intervalMin = 1f;
    public float intervalMax = 4f;

    static int[][] sequences = {
        new int[] { 0, 1, 0, 1 }, // blue
        new int[] { 0, 1, 2, 3 }, // red
        new int[] { 1, 3, 0, 2 }, // yellow
        new int[] { 3, 2, 1, 0 } // green
    };

    float timeNextSpawn;
    
    void Start()
    {
        timeNextSpawn = Time.time + UnityEngine.Random.Range(intervalMin, intervalMax);
    }
    
    void Update()
    {
        if (Time.time >= timeNextSpawn)
        {
            timeNextSpawn = Time.time + UnityEngine.Random.Range(intervalMin, intervalMax);
            Vector2 planarPos = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 pos = transform.position + new Vector3(planarPos.x, 1, planarPos.y);
            Passerby passerby = Instantiate(passerbyPrefab, pos, Quaternion.identity);
            passerby.musician = musician;
            passerby.sequence = new List<int>(sequences[Math.Min((int)UnityEngine.Random.Range(0, sequences.Length), sequences.Length - 1)]);
        }
    }
}
