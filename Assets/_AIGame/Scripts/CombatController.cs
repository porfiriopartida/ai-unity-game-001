using UnityEngine;

public class CombatController : MonoBehaviour
{
    public float attackRange = 1.0f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;

    public void Attack()
    {
        // A simple attack implementation using a physics overlap sphere to detect enemies
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        foreach (var hit in hits)
        {
            hit.GetComponent<HealthController>()?.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
