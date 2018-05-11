using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AddComponentIntoObject : MonoBehaviour {

    public int Amount = 9;

    public Vector3 Size;

    public float Osset9Edge = 1.7f;

	// Use this for initialization
    [ContextMenu("AddBoxCollider")]
	void AddCollider ()
    {
        var colliders = GetComponents<BoxCollider>();
        foreach( var col in colliders)
        {
            DestroyImmediate(col);
        }

        for ( int i = 0; i < Amount; i++)
        {
            var collider = this.gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3(Osset9Edge * ((i / 3 == 0) ? -1:  (i / 3)), Osset9Edge, Osset9Edge * ((i % 3 == 0) ? -1 : (i % 3)));
            collider.size = Size;
            collider.isTrigger = true;
        }
	    	
	}
}
