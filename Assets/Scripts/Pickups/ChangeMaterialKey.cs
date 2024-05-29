using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialKey : MonoBehaviour
{
    public float swapRange;

    MeshRenderer[] meshRenderers;
    Transform player;
    public Material[] materials;

    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < swapRange)
        {
            foreach (var renderer in meshRenderers)
            {
                var materialsCopy = renderer.materials;
                materialsCopy[0] = materials[1];
                renderer.materials = materialsCopy;
            }

        }
        else
        {
            foreach (var renderer in meshRenderers)
            {
                var materialsCopy = renderer.materials;
                materialsCopy[0] = materials[0];
                renderer.materials = materialsCopy;
            }
        }
    }
}
