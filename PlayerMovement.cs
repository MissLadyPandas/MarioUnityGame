using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private AudioManager audioManager;
    private new Collider2D collider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime /2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        camera = Camera.main;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Update()
    {
        if (!Pause.GameIsPaused)
        {
            HorizontalMovement();
                
            grounded = rigidbody.Raycast(Vector2.down);
            Debug.Log("Grounded: " + grounded);

            if (grounded) 
            {
                GroundedMovement();
            }

            ApplyGravity();
        }
    }


    private void HorizontalMovement()
    {
        inputAxis = Input.GetAxisRaw("Horizontal");
        Debug.Log("Input Axis: " + inputAxis);
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        if (rigidbody.Raycast(Vector2.right * velocity.x)) {
            velocity.x = 0f;
        }

        if (inputAxis > 0f) {
            transform.eulerAngles = Vector3.zero; 
        } else if (inputAxis < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f); 
        }
    }


    private void GroundedMovement()
    {
        if (!Pause.GameIsPaused)
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
            jumping = velocity.y > 0f;

            if (Input.GetButtonDown("Jump")) 
            {
                velocity.y = jumpForce;
                jumping = true;
                AudioManager.Instance.PlaySmallJump(); // Play the smallJump sound effect
            }
        }
    }




    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + .05f, rightEdge.x - .05f);

        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Check if the character is landing on top of the enemy
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Vector2.Dot(contact.normal, Vector2.up) > 0.5f)
                {
                    // Apply bounce force
                    velocity.y = jumpForce / 2f;
                    jumping = true;
                    break;
                }
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            // Handle other collisions, for example, with walls or platforms
            if (Vector2.Dot(collision.GetContact(0).normal, Vector2.up) > 0.5f)
            {
                velocity.y = 0f;
            }
        }
    }
}

