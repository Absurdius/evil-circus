using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public float crouchTime;
    float distance;

    public Transform crouchTarget;

    Vector3 direction;

    private void Update()
    {
        transform.Translate(direction.normalized * (Time.deltaTime * (distance / crouchTime)));
    }

    public bool Crouch(bool isCrouching)
    {
        if (isCrouching)
        {
            direction = crouchTarget.position - transform.position;
            distance = direction.magnitude;
        }

        return !isCrouching;
    }
}
