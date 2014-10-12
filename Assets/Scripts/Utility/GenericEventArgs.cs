using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Extends EventArgs to include any generic datatype as a passed member
/// </summary>
/// <typeparam name="T">Datatype of the object that will be passed</typeparam>
public class EventArgs<T> : EventArgs
{
    private readonly T eventData;

    /// <summary>
    /// Extends EventArgs to include any generic datatype as a passed member
    /// </summary>
    /// <param name="data">Object that is passed when the event is raised</param>
    public EventArgs(T data)
    {
        eventData = data;
    }

    /// <summary>
    /// Object that is passed when the event is raised
    /// </summary>
    public T Data
    {
        get
        {
            return eventData;
        }
    }
}
