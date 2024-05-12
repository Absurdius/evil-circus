using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Light flashlight;

    private void Start()
    {
        flashlight = GetComponent<Light>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight.isActiveAndEnabled)
            {
                flashlight.enabled = false;
            }
            else
            {
                flashlight.enabled = true;
            }
        }
    }
}
