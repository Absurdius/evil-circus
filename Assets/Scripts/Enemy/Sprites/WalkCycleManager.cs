using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WalkCycleManager : MonoBehaviour
{
    private readonly int FORWARD = 0, SIDE = 1, BACK = 2;
    private int currentDirection = 0;

    private float playerRelativeAngle = 0.0f;
    private Transform playerTransform;
    private Vector3 relativeVector = Vector3.forward;
    private SpriteRenderer spriteRenderer; 
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Orientation").transform;
        if (playerTransform == null) { Debug.LogError("WalkcycleManager: playerTransform Not found!"); }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { Debug.LogError("WalkcycleManager: spriterenderer Not found!"); }
        animator = GetComponent<Animator>();
        if (animator == null) { Debug.LogError("WalkcycleManager: animator Not found!"); }

    }

    // Update is called once per frame
    void Update()
    {
        relativeVector = playerTransform.position - transform.position;
        playerRelativeAngle = Vector3.SignedAngle(relativeVector, transform.forward, Vector3.up);
        if (playerRelativeAngle >= 135f || playerRelativeAngle <= -135f)
        {
            // back
            CheckAndUpdateSprite(BACK);
        }
        else if (playerRelativeAngle < 135f && playerRelativeAngle > 45f)
        {
            // left
            spriteRenderer.flipX = true;
            CheckAndUpdateSprite(SIDE);
        } 
        else if (playerRelativeAngle > -135f && playerRelativeAngle < -45f)
        {
            //right
            CheckAndUpdateSprite(SIDE);
        }
        else
        {
            // forward
            CheckAndUpdateSprite(FORWARD);
        } 
    }

    private void CheckAndUpdateSprite(int dir)
    {
        if(dir != currentDirection)
        {
            animator.SetInteger("direction", dir);
            Debug.Log("Switch direction" + dir );
        }
        currentDirection = dir;
    }

}
