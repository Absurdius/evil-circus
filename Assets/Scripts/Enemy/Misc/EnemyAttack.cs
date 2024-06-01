using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange;
    bool canAttack;
    public bool isAttacking;
    private Animator animator;
    public PlayerDeath playerDeath;
    private MeshRenderer meshRenderer;
    PlayerMovement playerMovement;
    //UIStateManager stateManager;
    AudioSource audioSource;

    private void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        //stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        canAttack = true;
    }

    private void Update()
    {
        if (isAttacking)
        {
            HitCheck();
        }

        if (audioSource.isPlaying && UIStateManager.currentState == UIStateManager.UIState.PAUSED)
        {
            audioSource.Stop();
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            isAttacking = true;
            /*
            meshRenderer.enabled = true;
            if (!playerMovement.isCrouching)
            {
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetTrigger("Attack2");
            }*/
            audioSource.Play();
        }
    }

    public void StunCancel()
    {
        animator.SetTrigger("Stun");
        StopAttacking();
        ResetAttack();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        animator.ResetTrigger("Stun");
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }

    public void ResetAttack()
    {
        canAttack = true;
        meshRenderer.enabled = false;
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Attack2");
    }

    private void HitCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange / 2, 1 << 8);
        foreach (var hitCollider in hitColliders)
        {
            playerDeath.Death();
        }
    }
}

