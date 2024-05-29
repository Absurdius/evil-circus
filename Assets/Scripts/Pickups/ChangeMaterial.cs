using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public float swapRange;

    MeshRenderer meshRenderer;
    Transform player;
    public Material[] materials;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        player = GameObject.FindWithTag("Player").transform;
    }


    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < swapRange)
        {
            var materialsCopy = meshRenderer.materials;
            materialsCopy[0] = materials[1];
            meshRenderer.materials = materialsCopy;
        }
        else
        {
            var materialsCopy = meshRenderer.materials;
            materialsCopy[0] = materials[0];
            meshRenderer.materials = materialsCopy;
        }
    }
}
