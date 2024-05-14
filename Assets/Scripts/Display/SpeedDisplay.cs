using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    Rigidbody rb;
    private TMP_Text display;

    private void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponentInChildren<Rigidbody>();

        display = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        display.text = "Speed " + rb.velocity.magnitude;
    }
}
