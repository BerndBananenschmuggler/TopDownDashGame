using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField][Range(0, float.MaxValue)] private float m_maximumHealthPoints = 100f;
    private float m_currentHealthPoints;

    public UnityEvent OnHealthChanged = null;
    public UnityEvent OnDied = null;

    private void Awake()
    {
        m_currentHealthPoints = m_maximumHealthPoints;
    }

    public void ApplyDamage(float damage)
    {
        m_currentHealthPoints -= damage;        

        if (m_currentHealthPoints <= 0)
            Die();

        HealthChanged();
    }

    public void Heal(float healing)
    {
        m_currentHealthPoints += healing;

        if (m_currentHealthPoints > m_maximumHealthPoints)
            m_currentHealthPoints = m_maximumHealthPoints;

        HealthChanged();
    }

    private void Die()
    {
        OnDied?.Invoke();

        // Trigger Destroy Action if possible
        GetComponent<DestroyAction>()?.Trigger();
    }

    protected virtual void HealthChanged()
    {
        Debug.Log($"Health: {m_currentHealthPoints}");

        OnHealthChanged?.Invoke();
    }
}
