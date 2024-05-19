using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : BaseObject
{
    public Vector3 CenterPosition => transform.position + Vector3.up * CurrentCollider.radius;
    public CircleCollider2D CurrentCollider;
}
