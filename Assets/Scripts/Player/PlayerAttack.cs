using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int weaponUses;
    public int remainingUses;

    public float attckSize;
    private bool canAttack = true;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        remainingUses = weaponUses;
    }

    void Update()
    {
        if (Input.GetButton("Attack") && canAttack && GameStateManager.currentState == GameStateManager.GameState.Playing)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
    }

    private void CollisionCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attckSize, 1 << 9);
        foreach (Collider collider in colliders)
        {
            StartCoroutine(collider.gameObject.GetComponentInParent<EnemyController>().ChangeState(0, EnemyController.EnemyState.Stunned));
        }
        if (colliders.Length > 0)
        {
            remainingUses--;
            if (remainingUses <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        animator.ResetTrigger("Attack");
    }
}
