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

    [ExecuteInEditMode]
    [ContextMenu("Test")]
    public void Test()
    {
        Event.Invoke();
    }



    private void OnMouseEnter()
    {
        var temp = this.gameObject.GetComponent<Renderer>();

        if (temp != null)
        {
            temp.sharedMaterial.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        var temp = this.gameObject.GetComponent<Renderer>();

        if (temp != null)
        {
            temp.sharedMaterial.color = Color.white;
        }
    }
    void OnMouseDown()
    {
        if(Mathf.Abs(Vector3.Distance(this.transform.position, Camera.main.transform.position)) < Distance)
        {
            Event.Invoke();
        }

    }
}
