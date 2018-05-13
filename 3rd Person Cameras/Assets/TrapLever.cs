using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLever : Trap {

    public string Tag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (CanStart)
        {
            if (other.gameObject.CompareTag(Tag))
            {
                this.PLay();
            }
        }
    }
}
