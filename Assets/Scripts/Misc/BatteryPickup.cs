using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I touched something");
        if (other.gameObject.layer == 8)
        {
            Debug.Log("I touched the player");
            other.gameObject.GetComponentInChildren<Flashlight>().AddBattery();
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}
