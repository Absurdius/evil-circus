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

    private void Start()
    {
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        canAttack = true;
    }

    private void Update()
    {
        if (isAttacking)
        {
            HitCheck();
        }
    }

    public void Attack()
    {
        if (canAttack)
        {
            canAttack = false;
            isAttacking = true;
            meshRenderer.enabled = true;
            animator.SetTrigger("Attack");
        }
    }

    public void StunCancel()
    {
        animator.SetTrigger("Stun");
        canAttack = true;
        isAttacking = false;
        meshRenderer.enabled = false;
        animator.ResetTrigger("Attack");
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
