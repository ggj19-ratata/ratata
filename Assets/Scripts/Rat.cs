using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rat : MonoBehaviour
{
    public string button0;
    public string button1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!String.IsNullOrEmpty(button0) && Input.GetButtonDown(button0))
            Debug.Log("0");
        if (!String.IsNullOrEmpty(button1) && Input.GetButtonDown(button1))
            Debug.Log("1");
    }
}
