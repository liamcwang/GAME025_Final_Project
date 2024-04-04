using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorType {CONTINUOUS, ON_START};

/// <summary>
/// Class to give enemy specific AI components
/// They must implement an "Act()" function that is called
/// when the behavior is needed
/// </summary>
public abstract class AIBehavior : MonoBehaviour
{
    public abstract BehaviorType behaviorType {get;}
    public abstract void Act();
}
