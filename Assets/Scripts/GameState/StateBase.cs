using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase<T>
{
    public T StateID;

    public virtual void OnEnter()
    {
        //Debug.Log("OnEnter " + StateID);

    }

    public virtual void OnUpdate()
    {
        Debug.Log("OnUpadte " + StateID);
    }

    public virtual void OnExit()
    {
       //Debug.Log("OnExit " + StateID);
    }
}
