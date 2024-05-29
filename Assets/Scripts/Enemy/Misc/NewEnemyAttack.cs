using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyAttack : MonoBehaviour
{
    public float attackRange;
    public float attackDelay;
    public float attackCooldown;
    public bool canAttack;
    public bool isAttacking = false;


    private Animator animator;
    private GameStateManager stateManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        stateManager = GameObject.Find("ScriptHolder").GetComponent<GameStateManager>();
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(Attacking());
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
        StopAllCoroutines();
    }

    public void ResetAttack()
    {
        isAttacking = false;
        canAttack = true;
        animator.ResetTrigger("Attack");
    }

    public IEnumerator Attacking()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        CollisionCheck();
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
