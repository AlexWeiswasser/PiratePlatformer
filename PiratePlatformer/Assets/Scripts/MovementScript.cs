using System.Collections;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
	// Components
	private Rigidbody2D rb;
	public PlayerControllerSettings settings;

	// Dashing
	private bool isDashing = false;
	private bool canDash = true;

	// Coyote Time Variables
	public float coyoteTime = 0.1f; 
	private float coyoteTimeCounter = 0f;

	// Movement Vectors
	private Vector2 movementVector = Vector2.zero;
	private Vector2 lastMovement = new Vector2(1, 0);
	private Vector2 startPos;

	// Layers
	private int floorLayer;
	private int birdLayer;

	// Cached Collider for ground checking
	private Collider2D col;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();

		floorLayer = LayerMask.GetMask("Floor");
		birdLayer = LayerMask.GetMask("Parrot");

		startPos = transform.position;
	}

	void Update()
	{
		movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (movementVector != Vector2.zero)
		{
			lastMovement = movementVector;
		}

		if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f && !isDashing)
		{
			Jumping();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
		{
			Dashing();
		}

		if (coyoteTimeCounter > 0f)
		{
			coyoteTimeCounter -= Time.deltaTime;
		}
	}

	void FixedUpdate()
	{
		if (!isDashing)
		{
			Walking();
			GroundCheck();
		}
	}

	void Walking()
	{
		rb.velocity = new Vector2(movementVector.x * settings.walkSpeed, rb.velocity.y);
	}

	void Jumping()
	{
		coyoteTimeCounter = 0f;
		rb.velocity = new Vector2(rb.velocity.x, settings.jumpForce);
	}

	void Dashing()
	{
		rb.gravityScale = 0f;
		rb.velocity = lastMovement.normalized * settings.dashForce;
		isDashing = true;
		canDash = false;
		StartCoroutine(OnDash());
	}

	void GroundCheck()
	{
		Vector2 rayOrigin = new Vector2(col.bounds.center.x, col.bounds.min.y);
		RaycastHit2D floorHit = Physics2D.Raycast(rayOrigin, Vector2.down, settings.canJumpDistance, floorLayer);
		RaycastHit2D birdHit = Physics2D.Raycast(rayOrigin, Vector2.down, settings.canJumpDistance, birdLayer);

		if (floorHit.collider != null || birdHit.collider != null)
		{
			coyoteTimeCounter = coyoteTime; 
			canDash = true;
		}
	}

	IEnumerator OnDash()
	{
		yield return new WaitForSeconds(settings.dashTime);
		rb.gravityScale = 3f;
		isDashing = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Danger")
		{
			transform.position = startPos;
		}
	}
}
