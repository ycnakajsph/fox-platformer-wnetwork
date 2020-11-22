using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eagle : enemy
{

    private bool facingLeft = true;
    [SerializeField] private float leftCap = 26f;
    [SerializeField] private float rightCap = 40f;
    [SerializeField] private float idle_speed = 5f;
    private BoxCollider2D collid;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collid = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // If player within a range : we pike on him, anim == attack
        // else anim = eagle, and we go right and left
        

        if (facingLeft)
        {
            // Check if we are beyong leftCap
            if (transform.position.x > leftCap)
            {
                transform.localScale = new Vector2(1, 1);
                rb.velocity = new Vector2(-idle_speed, 0);

            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                transform.localScale = new Vector2(-1, 1);
                rb.velocity = new Vector2(+idle_speed, 0);
            }
            else
            {
                facingLeft = true;
            }
        }

    }
}
