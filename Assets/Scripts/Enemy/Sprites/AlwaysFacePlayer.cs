using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFacePlayer : MonoBehaviour
{
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.Find("Orientation").transform;
        if (playerTransform == null) { Debug.LogError("WalkcycleManager: playerTransform Not found!"); }
    }

    void Update()
    {
        transform.forward = transform.position - playerTransform.position;
    }
}
