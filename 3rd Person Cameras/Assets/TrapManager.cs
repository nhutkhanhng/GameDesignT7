using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour {

    public List<Trap> listTrap;

    public bool canPlay = true;


    public void ShutUpAllTrap()
    {
        foreach(Trap trap in listTrap)
        {
            trap.CanStart = false;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
