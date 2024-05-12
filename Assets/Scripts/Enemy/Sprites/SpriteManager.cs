using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public GameObject forward;
    public GameObject backward;
    public GameObject leftSide;
    public GameObject rightSide;

    private Transform playerTransform;
    //private MeshRenderer meshRenderer;

    //private Sprite current; 
    //private Sprite next; 

    //private float dotProduct = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Orientation").transform;
        if (playerTransform == null) { Debug.LogError("Spritemanager: playerTransform Not found!"); }
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //meshRenderer = GetComponentInChildren<MeshRenderer>();
        //meshRenderer.material = backward;
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

        var relativePos = playerTransform.position - transform.position;
        var fwd = transform.forward;
        var angle = Vector3.Angle(relativePos, fwd);

        if (angle >= 135f)
        {
            Reset();
            backward.SetActive(true);
        }
        else if (angle < 135f && angle > 45f)
        {
            if (Vector3.Cross(fwd, relativePos).y > 0)
            {
                Reset();
                leftSide.SetActive(true);
            }
            else
            {
                Reset();
                rightSide.SetActive(true);
            }
        }
        else
        {
            Reset();
            forward.SetActive(true);
        }
    }

    private void Reset()
    {
        forward.SetActive(false);
        leftSide.SetActive(false);
        rightSide.SetActive(false);
        backward.SetActive(false);
    }
}
