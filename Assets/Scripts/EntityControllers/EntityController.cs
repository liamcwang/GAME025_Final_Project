using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public ActionState state;
    public bool stateOverride;

    public Vector2 motionInput = Vector2.zero;
}
