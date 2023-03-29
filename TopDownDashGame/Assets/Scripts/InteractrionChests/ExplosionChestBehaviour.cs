using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionChestBehaviour : MonoBehaviour
{
    [SerializeField] private float m_collisionDamage = 33.4f;

    public UnityEvent OnExplode = null;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.ApplyDamage(m_collisionDamage);
            OnExplode?.Invoke();
        }
    }
}
