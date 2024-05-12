using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange;
    private bool canAttack;
    private bool isAttacking;
    private Animator animator;
    //public EnemyController enemyController;

    private void Start()
    {
        canAttack = true;
        isAttacking = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && canAttack)
        {
            Attack();
        }

        if (isAttacking)
        {
            HitCheck();
        }
    }

    public void Attack()
    {
        canAttack = false;
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    public void ResetAttack()
    {   
        canAttack = true;
        isAttacking = false;
        animator.ResetTrigger("Attack");
    }

    public void HitCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange / 2, 1 << 9);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider hitCollider = hitColliders[i];
            GameObject hitColliderObject = hitCollider.transform.parent.gameObject;
            hitColliderObject.GetComponent<EnemyController>().Stun();
        }
    }
}
