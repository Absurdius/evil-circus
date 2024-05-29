using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAttack : MonoBehaviour
{
    public float attackRange;
    public float attackCooldown;
    public bool canAttack;
    public bool isAttacking;


    private Animator animator;
    private GameStateManager stateManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        stateManager = GameObject.Find("ScriptHolder").GetComponent<GameStateManager>();
    }

    public void Attack()
    {
        if (canAttack)
        {
            isAttacking = true;
            canAttack = false;
            animator.SetTrigger("Attack");
        }
    }

    public void CollisionCheck()
    {
        if (Physics.CheckSphere(transform.position, attackRange, 1 << 8))
        {
            stateManager.DisplayDeathScreen();
        }
    }

    public void StopAttacking()
    {
        animator.SetTrigger("CancelAttack");
        ResetAttack();
        animator.ResetTrigger("CancelAttack");
    }

    public void ResetAttack()
    {
        isAttacking = false;
        canAttack = true;
        animator.ResetTrigger("Attack");
    }

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        ResetAttack();
    }
}
