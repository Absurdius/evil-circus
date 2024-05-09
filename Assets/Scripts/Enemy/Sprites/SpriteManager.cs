using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public Sprite forward;
    public Sprite backward;
    public Sprite side;

    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;

    private Sprite current; 
    private Sprite next; 

    private float dotProduct = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Orientation").transform;
        if (playerTransform == null) { Debug.LogError("Spritemanager: playerTransform Not found!"); }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = forward;
        current = forward;
    }

    // Update is called once per frame
    void Update()
    {
        dotProduct = Vector3.Dot(transform.forward, playerTransform.forward);

        //Debug.Log(dotProduct);

        if (dotProduct > 0.7f)
        {
            next = backward;
        } else if (dotProduct < 0.7f && dotProduct > -0.7f )
        {
            next = side;
        } else
        {
            next = forward;
        }



        if (current != next) {
            //Debug.Log("Changes sprite");
            current = next;
            spriteRenderer.sprite = current;
        }
    }   

}
