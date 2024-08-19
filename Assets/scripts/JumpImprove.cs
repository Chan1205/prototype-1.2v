using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpImprove : MonoBehaviour
{
    public float fallMultiplier = 4.5f;
    public float lowJumpMultiplier = 3f;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigid.velocity.y < 0)
            rigid.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rigid.velocity.y > 0 && !Input.GetButton("Jump"))
            rigid.velocity = new Vector2(rigid.velocity.x, 0) * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

}
