using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private TextMeshProUGUI text;

    void Start()
    {
        playerAttack = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerAttack>();    
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.text = "W: " + playerAttack.remainingUses;
    }
}
