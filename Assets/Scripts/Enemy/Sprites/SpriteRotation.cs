using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public Transform target;
    public Vector3 rotationMask;

    void Update()
    {
        //var relativPos = (target.position + new Vector3(0, 1, 0)) - transform.position;
        //transform.rotation = Quaternion.LookRotation(relativPos, Vector3.up);
        Vector3 lookAtRotation = Quaternion.LookRotation(target.position - transform.position).eulerAngles;
        transform.rotation = Quaternion.Euler(Vector3.Scale(lookAtRotation, rotationMask));
    }
}
