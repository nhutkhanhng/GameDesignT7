using System.Collections;   
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MissionTransform
{
    public Transform Obj;
    public Vector3 Value;

    public UnityEvent Event;

    public bool IsComplete()
    {
        Debug.LogError(Obj.rotation.eulerAngles.ToString());

        if (Obj.rotation.eulerAngles == Value)
            return true;

        return false;
    }

    public void Completed()
    {
        if (IsComplete())
        {
            Event.Invoke();
        }
    }

    public void Invoke()
    {
        Event.Invoke();
    }
}

public class MissionCondition : MonoBehaviour {

    public int indexMission;

    public List<MissionTransform> Mission;

    MissionTransform currentMission;

    private void Start()
    {
        indexMission = 0;

        if (Mission != null && Mission.Count >= 1)
            currentMission = Mission[0];


    }

    public void UpdateMission(int delta)
    {
        indexMission += delta;

        indexMission = (int)Mathf.Clamp(indexMission, 0, Mission.Count - 1);

        if (indexMission >= 0 && indexMission < Mission.Count)
        {
            currentMission = Mission[indexMission];
        }
    }
    public void CheckedOut()
    {
        if (currentMission != null)
        {
            if (currentMission.IsComplete())
            {
                Debug.LogError("SẠDKJSADKLD");

                currentMission.Invoke();
            }
        }


    }
}
