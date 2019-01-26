using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasserbySpawner : MonoBehaviour
{
    public Musician musician;
    public Passerby passerbyPrefab;
    public float radius = 1f;
    public float intervalMin = 1f;
    public float intervalMax = 4f;

    float timeNextSpawn;
    
    void Start()
    {
        timeNextSpawn = Time.time + Random.Range(intervalMin, intervalMax);
    }
    
    void Update()
    {
        if (Time.time >= timeNextSpawn)
        {
            timeNextSpawn = Time.time + Random.Range(intervalMin, intervalMax);
            Vector2 planarPos = Random.insideUnitCircle * radius;
            Vector3 pos = transform.position + new Vector3(planarPos.x, 1, planarPos.y);
            Passerby passerby = Instantiate(passerbyPrefab, pos, Quaternion.identity);
            passerby.musician = musician;
        }
    }
}
