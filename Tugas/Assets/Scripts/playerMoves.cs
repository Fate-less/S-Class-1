using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoves : Player
{
    public float speed;
    Rigidbody2D body;
    SpriteRenderer sprite;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.velocity = new Vector2(speed, body.velocity.y);
            sprite.flipX = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.velocity = new Vector2(-speed, body.velocity.y);
            sprite.flipX = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }
}