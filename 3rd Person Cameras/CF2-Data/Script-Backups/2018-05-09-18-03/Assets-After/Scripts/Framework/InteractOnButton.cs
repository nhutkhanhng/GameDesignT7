using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractOnButton : InteractOnTrigger
{

    public string buttonName = "X";
    public UnityEvent OnButtonPress;

    bool m_CanExecuteButtons = false;

    protected override void ExecuteOnEnter(Collider other)
    {
        m_CanExecuteButtons = true;
    }

    protected override void ExecuteOnExit(Collider other)
    {
        m_CanExecuteButtons = false;
    }

    void Update()
    {
        if (m_CanExecuteButtons && ControlFreak2.CF2Input.GetButtonDown(buttonName))
        {
            OnButtonPress.Invoke();
        }
    }

}

