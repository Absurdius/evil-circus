using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WalkCycleManager : MonoBehaviour
{
    private readonly int FORWARD = 0, SIDE = 1, BACK = 2;
    private int currentDirection = 0;

    private float playerRelativeAngle = 0.0f;
    private Transform playerTransform;
    private Vector3 relativeVector = Vector3.forward;
    private SpriteRenderer spriteRenderer; 
    private Animator animator;
    public Sprite[] sprites;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        if (playerTransform == null) { Debug.LogError("WalkcycleManager: playerTransform Not found!"); }
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null) { Debug.LogError("WalkcycleManager: spriterenderer Not found!"); }
        animator = GetComponentInChildren<Animator>();
        if (animator == null) { Debug.LogError("WalkcycleManager: animator Not found!"); }
        agent = GetComponentInParent<NavMeshAgent>();

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
            spriteRenderer.sprite = sprites[2];
        }
        else if (playerRelativeAngle < 135f && playerRelativeAngle > 45f)
        {
            // left
            spriteRenderer.flipX = false;
            CheckAndUpdateSprite(SIDE);
            spriteRenderer.sprite = sprites[1];
        } 
        else if (playerRelativeAngle > -135f && playerRelativeAngle < -45f)
        {
            //right
            spriteRenderer.flipX = true;
            CheckAndUpdateSprite(SIDE);
            spriteRenderer.sprite = sprites[1];
        }
        else
        {
            // forward
            CheckAndUpdateSprite(FORWARD);
            spriteRenderer.sprite = sprites[0];
        }

        Debug.Log(agent.velocity.magnitude);
        
        if (agent.velocity.magnitude < 0.1)
        {
            animator.SetBool("IsStopped", true);
        }
        else
        {
            animator.SetBool("IsStopped", false);
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
