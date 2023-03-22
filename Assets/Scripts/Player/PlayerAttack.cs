using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // --- SERIALIZED FIELDS --- //
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackRange;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    
    // --- FIELDS --- //
    private float nextAttack;
    private PlayerController playerController;
    
    // --- UNITY METHODS --- //
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (nextAttack > 0)
        {
            nextAttack -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && nextAttack <= 0)
        {
            playerController.ChangeState(PlayerState.Attacking);
            playerController.CompleteAttack();
            nextAttack = attackRate;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // --- PUBLIC METHODS --- //
    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                enemyStats.TakeDamage(attackDamage);
            }
        }
    }   
}
