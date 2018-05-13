using System.Collections;
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
        EventTrap.Invoke();
    }

    public virtual void Stop()
    {
        CanStart = false;
    }

    public virtual void TriggerEnter()
    {
        if (CanStart)
            PLay();
    }

}
