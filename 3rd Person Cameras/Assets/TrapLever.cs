using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLever : Trap {


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.PLay();
        }
    }
}
