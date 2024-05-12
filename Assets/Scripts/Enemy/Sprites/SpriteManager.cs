using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public Sprite forward;
    public Sprite backward;
    public Sprite leftSide;
    public Sprite rightSide;

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
        //spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = forward;
        current = forward;
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */

        dotProduct = Vector3.Dot(transform.forward, playerTransform.forward);

        var relativePos = playerTransform.position - transform.position;
        var fwd = transform.forward;
        var angle = Vector3.Angle(relativePos, fwd);

        if (angle >= 135f)
        {
            spriteRenderer.sprite = backward;
        }
        else if (angle < 135f && angle > 45f)
        {
            if (Vector3.Cross(fwd, relativePos).y > 0)
            {
                spriteRenderer.sprite = leftSide;
            }
            else
            {
                spriteRenderer.sprite = rightSide;
            }
        }
        else
        {
            spriteRenderer.sprite = forward;
        }
    }   

}
