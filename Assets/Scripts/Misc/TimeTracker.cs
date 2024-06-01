using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private GameObject finalScoreObject;
   
    private float survivalTime = 0f;
    private bool takingTime = true;

    void Start()
    {
        survivalTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (takingTime) { 
        survivalTime += Time.deltaTime;
        }
    }

    public void StopTimeTracking()
    {
        takingTime = false;
        survivalTime = Mathf.Floor(survivalTime);
        Debug.Log("Time survived" + survivalTime);
        TextMeshProUGUI txt = finalScoreObject.GetComponent<TextMeshProUGUI>();
        if(txt != null)
        { 
            txt.text = survivalTime.ToString();
        }
    }

    public void ResetTimeTracking()
    {
        
    }
}
