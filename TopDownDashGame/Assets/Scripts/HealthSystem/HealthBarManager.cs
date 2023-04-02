using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Health m_health;
    [SerializeField] private GameObject m_healthBar;

    private void Awake()
    {
        if (m_health == null)
            m_health = GetComponent<Health>();
    }

    private void Start()
    {
        m_health.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged()
    {
        //m_healthBar.transform.
        Image healthBarSprite = m_healthBar.GetComponent<Image>();
        healthBarSprite.fillAmount = m_health.CurrentHealth / m_health.MaxHealth;
    }
}
