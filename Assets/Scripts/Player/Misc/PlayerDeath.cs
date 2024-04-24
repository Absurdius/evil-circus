using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Transform respawnPoint;

    public void Death()
    {
        Debug.Log("You died!");

        transform.position = respawnPoint.position;
    }
}
