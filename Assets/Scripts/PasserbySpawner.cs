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
    public float[] times = new float[] { 0f, 0f, 0f, 0f };

    static int[][] sequences = {
        new int[] { 0, 1, 0, 1 }, // blue
        new int[] { 0, 1, 2, 3 }, // red
        new int[] { 1, 3, 0, 2 }, // yellow
        new int[] { 3, 2, 1, 0 } // green
    };

    float timeStart;
    float timeNextSpawn;
    
    void Start()
    {
        timeStart = Time.time;
        Debug.Assert(Mathf.Min(times) >= 0);
        timeNextSpawn = Time.time + Mathf.Min(times) + UnityEngine.Random.Range(0, intervalMax - intervalMin);
    }
    
    void Update()
    {
        if (Time.time >= timeNextSpawn)
        {
            timeNextSpawn = Time.time + UnityEngine.Random.Range(intervalMin, intervalMax);
            Spawn();
        }
    }

    List<int> ValidSequences(float timeFromStart)
    {
        List<int> result = new List<int>();
        if (timeFromStart >= times[0]) result.Add(0);
        if (timeFromStart >= times[1]) result.Add(1);
        if (timeFromStart >= times[2]) result.Add(2);
        if (timeFromStart >= times[3]) result.Add(3);
        return result;
    }

    void Spawn()
    {
        Vector2 planarPos = UnityEngine.Random.insideUnitCircle * radius;
        Vector3 pos = transform.position + new Vector3(planarPos.x, 1, planarPos.y);
        Passerby passerby = Instantiate(passerbyPrefab, pos, Quaternion.identity);
        passerby.musician = musician;
        var sequenceIndices = ValidSequences(Time.time - timeStart);
        int sequenceIndex = sequenceIndices[Math.Min((int)UnityEngine.Random.Range(0, sequenceIndices.Count), sequenceIndices.Count - 1)];
        passerby.SetSequence(new List<int>(sequences[sequenceIndex]));
    }
}
