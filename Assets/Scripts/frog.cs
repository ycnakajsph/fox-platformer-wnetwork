using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : enemy
{
    [SerializeField] private LayerMask ground;
    private BoxCollider2D collid;

    [SerializeField] private float leftCap = -25f;
    [SerializeField] private float rightCap = -12f;

    [SerializeField] private float jumpLength = 5f;
    [SerializeField] private float jumpHeight = 5f;

    private bool jumping = false;
    private bool falling= false;

    private bool facingLeft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collid = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // From jump to fall
        if (anim.GetBool("Jumping") == true)
        {
            if (rb.velocity.y < .1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }

        // From Fall to idle
        if (collid.IsTouchingLayers(ground) && anim.GetBool("Falling") == true)
        {
            anim.SetBool("Falling", false);
        }
    }

    private void Move()
    {
        if (facingLeft)
        {
            // Check if we are beyong leftCap
            if (transform.position.x > leftCap)
            {
                transform.localScale = new Vector2(1, 1);
                if (collid.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    jumping = true;
                }
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
                if (collid.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    jumping = true;
                }
            }
            else
            {
                facingLeft = true;
            }
        }
        anim.SetBool("Jumping", jumping);
    }

    
   
}
