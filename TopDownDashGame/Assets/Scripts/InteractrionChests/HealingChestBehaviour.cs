using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealingChestBehaviour : MonoBehaviour
{
    [SerializeField] private float m_collisionHealing = 33.4f;

    public UnityEvent OnUsed = null;

    private void OnTriggerEnter(Collider other)
    {
        IHealable healable = other.gameObject.GetComponent<IHealable>();

        if (healable != null)
        {
            healable.Heal(m_collisionHealing);
            OnUsed?.Invoke();
        }
    }
}
