using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour 
{

    [Header("Distance form MainCamera for click Object")]
    public float Distance = 5f;
    public UnityEvent Event;

    void OnMouseDown()
    {
        if(Mathf.Abs(Vector3.Distance(this.transform.position, Camera.main.transform.position)) < Distance)
        {
            Event.Invoke();
        }

    }
}
