using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        var relativPos = (target.position + new Vector3(0, 1, 0)) - transform.position;
        transform.rotation = Quaternion.LookRotation(relativPos);
        //transform.Rotate(0, transform.position.y, 0, Space.World);
        //transform.Rotate(0, 0, 0, Space.Self);
    }
}
