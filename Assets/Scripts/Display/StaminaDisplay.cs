using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaDisplay : MonoBehaviour
{
    //public PlayerMovement playerMovement;
    PlayerMovement playerMovement;
    private TMP_Text display;

    private void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        display = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        display.text = "Stamina " + playerMovement.stamina + "/100";
    }
}
