using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    public bool Crouch(bool isCrouching, float playerHeight, float crouchModifier)
    {
        if (!isCrouching)
        {
            transform.position = transform.position + new Vector3(0, -playerHeight * crouchModifier, 0);
        }
        else if (isCrouching)
        {
            transform.position = transform.position + new Vector3(0, playerHeight * crouchModifier, 0);
        }

        return !isCrouching;
    }
}
