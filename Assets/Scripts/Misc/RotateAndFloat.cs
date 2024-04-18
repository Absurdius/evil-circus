using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndFloat : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float hoverSpeed = 1.0f;

 

    // Update is called once per frame
    void Update()
    {
        // twist the object transform 
        this.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        // manipulate the height
        this.transform.Translate(Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * Time.deltaTime);
    }
}
