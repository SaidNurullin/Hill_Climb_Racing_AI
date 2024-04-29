using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageTaker : MonoBehaviour
{
    [NonSerialized] public UnityEvent OnDieing = new UnityEvent();
    private bool _is_alive;
    private void Awake()
    {
        _is_alive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_is_alive) return;

        Debug.Log("CarTakeDamage");
        Die();
    }

    public bool IsAlive { get { return _is_alive; } }

    private void Die()
    {
        _is_alive = false;
        OnDieing.Invoke();
    }
}
