using UnityEngine;
using UnityEngine.UI;
public class playerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D collid;
    
    private enum State {idle, running, jumping, falling, hurt };
    [SerializeField] private State state = State.idle;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump_speed = 15f;
    [SerializeField] private float hurt_speed = 5f;
    [SerializeField] private int cherry = 0;
    [SerializeField] private Text cherry_number;

    [SerializeField] private AudioSource cherry_sound;
    [SerializeField] private AudioSource footstep;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collid = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.hurt)
        {
            handle_input_dir();
        }

        AnimStateswitch();
        /*if (state == State.running)
        {
            trigfootsteps();
        }*/
    }

    private void handle_input_dir()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(+speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && collid.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, +jump_speed);
            state = State.jumping;
        }
    }

    private void AnimStateswitch ()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling && collid.IsTouchingLayers(ground))
        {
            state = State.idle;
        }
        else if (state == State.hurt )
        {
            if (Mathf.Abs(rb.velocity.x) < .1f )
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f && state != State.falling)
        {
            // mv r or l
            state = State.running;
        }
        else if (Mathf.Abs(rb.velocity.x) < 2f && state == State.running)
        {
            state = State.idle;
        }
        
        anim.SetInteger("State", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collectable")
        {
            cherry_sound.Play();
            Destroy(collision.gameObject);
            cherry += 1;
            cherry_number.text = cherry.ToString();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemy Enemy = collision.gameObject.GetComponent<enemy>();
            if (state == State.falling)
            {
                //Destroy(collision.gameObject);
                Enemy.JumpedOn();
            }
            else
            {
                state = State.hurt;
                if (collision.gameObject.transform.position.x > transform.position.x)
                {
                    // This means enemy is on the right
                    // Then throw me on the left
                    rb.velocity = new Vector2(-hurt_speed + 0.5f * rb.velocity.x, 0.5f*rb.velocity.y+ hurt_speed);
                    transform.localScale = new Vector2(-1, 1);

                    print("shall be sent left !");
                }
                else
                {
                    // Enemy is on the left
                    // Then throw me on the right 
                    rb.velocity = new Vector2(+hurt_speed+ 0.5f * rb.velocity.x, 0.5f*rb.velocity.y + hurt_speed);
                    transform.localScale = new Vector2(1, 1);

                    print("shall be sent right !");
                }
            }
        }
    }

    private void trigfootsteps()
    {
        //if (!footstep.isPlaying)
        //{
            footstep.Play();
        //}
    }
}

