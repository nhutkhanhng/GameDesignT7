﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour {

    public UnityEvent EventTrap;
    protected Collider trigger;
    public bool CanStart = true;

	public virtual void PLay()
    {
        Debug.LogError("Play");
        EventTrap.Invoke();
    }

    public virtual void Stop()
    {
    }

    public virtual void TriggerEnter()
    {
        if (CanStart)
            PLay();
    }

}