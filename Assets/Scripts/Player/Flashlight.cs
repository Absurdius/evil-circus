using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool isEnabled = false;
    private bool hasEnergy = true;
    public float maxEnergy;
    public float energyDrain;
    public float remainingEnergy;
    public int batteries;

    private Light flashlight;


    void Start()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = false;
        remainingEnergy = maxEnergy;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight.enabled)
            {
                flashlight.enabled = false;
                isEnabled = false;
            }
            else if (!flashlight.enabled && hasEnergy)
            {
                flashlight.enabled = true;
                isEnabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (batteries > 0 && remainingEnergy != maxEnergy)
            {
                batteries--;
                remainingEnergy = maxEnergy;
                if (!hasEnergy)
                {
                    hasEnergy = true;
                }
            }
        }

        if (isEnabled)
        {
            remainingEnergy -= energyDrain * Time.deltaTime;
        }

        if (remainingEnergy <= 0)
        {

            hasEnergy = false;
            flashlight.enabled = false;
            isEnabled = false;

        }
    }

    public void AddBattery()
    {
        batteries++;
    }
}
