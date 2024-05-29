using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVictory : MonoBehaviour
{
    public int requiredKeys;

    GameStateManager stateManager;
    KeyInventory inventory;

    private void Start()
    {
        stateManager = GameObject.Find("ScriptHolder").GetComponent<GameStateManager>();
        inventory = GameObject.FindWithTag("Player").GetComponent<KeyInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && inventory.keys == requiredKeys)
        {
            stateManager.DisplayVictoryScreen();
        }

        Debug.Log("detected collision. layermask is " + other.gameObject.layer);
    }
}
