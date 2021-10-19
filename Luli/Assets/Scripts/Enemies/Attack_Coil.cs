using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Coil : Attack
{
    public Rigidbody2D rb;
    public float forceToJump = 1;

    protected override void Start()
    {
        rb.AddForce(forceToJump * Vector2.up);
        base.Start();
    }
}
