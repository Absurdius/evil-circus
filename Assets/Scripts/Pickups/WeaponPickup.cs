using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private GameObject weapon;

    private void Awake()
    {
        weapon = GameObject.Find("PlayerWeapon");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (!weapon.activeSelf)
            {
                weapon.SetActive(true);
                this.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
