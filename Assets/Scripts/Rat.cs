using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Rat : MonoBehaviour
{
    public string button0;
    public string button1;
    public GameObject key0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!String.IsNullOrEmpty(button0) && Input.GetButtonDown(button0))
            ExecuteEvents.Execute<IHitMessageTarget>(key0, null, (x, y) => x.Hit());
        if (!String.IsNullOrEmpty(button1) && Input.GetButtonDown(button1))
            Debug.Log("1");
    }
}
