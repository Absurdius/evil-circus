using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class EyeSync : MonoBehaviour
{
    Light[] eyes;
    GapingController controller;

    private void Start()
    {
        controller = GetComponentInParent<GapingController>();
        eyes = GetComponentsInChildren<Light>();

        foreach (Light eye in eyes)
        {
            eye.range = controller.ScanDistance;
            eye.spotAngle = controller.scanAngle;
        }
    }
}
