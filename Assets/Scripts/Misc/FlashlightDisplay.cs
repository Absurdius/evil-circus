using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashlightDisplay : MonoBehaviour
{
    private Flashlight flashlight;
    private TextMeshProUGUI text;

    void Start()
    {
        flashlight = GameObject.FindWithTag("Player").GetComponentInChildren<Flashlight>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "B: " + flashlight.batteries + " E: " + flashlight.remainingEnergy.ToString("F1") + "/" + flashlight.maxEnergy;
    }
}
