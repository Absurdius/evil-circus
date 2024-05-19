using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 1 << 8)
        {
            var weapon = GameObject.Find("PlayerWeapon");
            if (!weapon.activeSelf)
            {
                weapon.SetActive(true);
                this.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
