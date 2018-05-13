using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AddComponentIntoObject : MonoBehaviour {

    public int Amount = 9;

    public int Edge;

    public Vector3 Size;

    public float Osset9Edge = 1.7f;

	// Use this for initialization
    [ContextMenu("AddBoxCollider")]
	void AddCollider ()
    {
        for ( int i = 0; i < Amount; i++)
        {
            var collider = this.gameObject.AddComponent<BoxCollider>();

            float row;

            if (i / Edge == 0)
            {
                row = -1f;
            }
            else if (i / Edge == 1)
            {
                row = 0f;
            }
            else
                row = 1f;

            float column;

            if (i % Edge == 0)
            {
                column = -1f;
            }
            else if (i % Edge == 1)
            {
                column = 0f;
            }
            else
                column = 1f;

            collider.center = new Vector3(Osset9Edge * row, Osset9Edge, Osset9Edge * column);
            collider.size = Size;
            collider.isTrigger = true;
        }
	    	
	}

    [ContextMenu("Clear")]
    void ClearCollider()
    {
        var colliders = GetComponents<BoxCollider>();
        foreach (var col in colliders)
        {
            DestroyImmediate(col);
        }
    }
}
