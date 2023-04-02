using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyAction : MonoBehaviour
{
    public UnityEvent OnDie;

    public void Trigger()
    {
        OnDie?.Invoke();

        Destroy(gameObject);
    }
}
