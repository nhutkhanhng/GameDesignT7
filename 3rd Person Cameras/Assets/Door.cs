using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour {

    public float doorOpenAngle = 90.0f;
    public float doorCloseAngle = 0.0f;
    public float doorAnimSpeed = 2.0f;
    private Quaternion doorOpen = Quaternion.identity;
    private Quaternion doorClose = Quaternion.identity;
    private Transform playerTrans = null;
    public bool doorStatus = false; //false is close, true is open
    private bool doorGo = false; //for Coroutine, when start only one

    public Transform Target;

    void Start()
    {
        doorStatus = false; //door is open, maybe change
                            //Initialization your quaternions
        doorOpen = Quaternion.Euler(0, doorOpenAngle, 0);
        doorClose = Quaternion.Euler(0, doorCloseAngle, 0);
        //Find only one time your player and get him reference
        //playerTrans = GameObject.Find("Player").transform;

        if (Target == null)
        {
            Target = Camera.main.transform;
        }
    }

    public void Play()
    {
        if ( doorStatus)
        {
            OpenTheDoor();
        }
        else
        {
            CloseTheDoor();
        }
    }
    [ExecuteInEditMode]
    [ContextMenu("Open")]
    public void OpenTheDoor()
    {
        StartCoroutine(moveDoor(doorOpen));
    }

    [ExecuteInEditMode]
    [ContextMenu("Close")]
    public void CloseTheDoor()
    {
        StartCoroutine(moveDoor(doorClose));
    }
    public IEnumerator moveDoor(Quaternion dest)
    {
        doorGo = true;
        //Check if close/open, if angle less 4 degree, or use another value more 0
        while (Quaternion.Angle(transform.localRotation, dest) > 4.0f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, dest, Time.deltaTime * doorAnimSpeed);
            //UPDATE 1: add yield
            yield return null;
        }
        //Change door status
        doorStatus = !doorStatus;
        doorGo = false;
        //UPDATE 1: add yield
        yield return null;
    }
}
