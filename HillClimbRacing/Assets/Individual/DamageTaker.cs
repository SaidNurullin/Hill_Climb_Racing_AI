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
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CarTakeDamage");
        _is_alive = false;
        OnDieing.Invoke();
    }

    public bool IsAlive { get { return _is_alive; } }
}
