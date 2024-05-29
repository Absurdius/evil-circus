using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerAttack : MonoBehaviour
{
    public int weaponUses;
    public int remainingUses;

    public float attckSize;
    private bool canAttack = true;

    private Animator animator;
    private AudioSource audioSource;
    private GameObject weaponDisplay;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //weaponDisplay = GameObject.Find("WeaponDisplay");
        //weaponDisplay.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        remainingUses = weaponUses;
        //weaponDisplay.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButton("Attack") && canAttack && UIStateManager.currentState == UIStateManager.UIState.PLAYING)
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
            StartCoroutine(collider.gameObject.GetComponentInParent<NewEnemyController>().ChangeState(0, NewEnemyController.EnemyState.Stunned));
        }
        if (colliders.Length > 0)
        {
            audioSource.Play();
            remainingUses--;
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        animator.ResetTrigger("Attack");
        if (remainingUses <= 0)
        {
            weaponDisplay.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
