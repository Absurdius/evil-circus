using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminatedCheck : MonoBehaviour
{
    private Transform moon;
    public bool isIlluminated;
    public bool debug;

    void Start()
    {
        moon = GameObject.FindWithTag("Moon").transform;
    }

    private void Update()
    {
        if (debug)
        {
            Debug.Log("I am illuminated: " + isIlluminated);
        }
    }

    void FixedUpdate()
    {
        var relativePos = moon.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, relativePos, out hit))
        {
            if (hit.collider.transform.gameObject.tag == "Moon")
            {
                isIlluminated = true;
                return;
            }
        }
        isIlluminated = false;
    }
}
