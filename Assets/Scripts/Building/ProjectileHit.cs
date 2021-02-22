using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    public float damage = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other != null)
            {
                other.GetComponent<WaspAI>().TakeDamage(damage);
            }
        }
    }
}
