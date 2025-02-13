using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

	//Components
	private Rigidbody2D rb;
	private SpriteRenderer playerRend;
	public PlayerControllerSettings settings;

	//Booleans
	private bool canJump = true;
	private bool isDashing = false;
	private bool canDash = true; 

	//Vectors
	private Vector2 movementVector = new Vector2(0, 0);
	private Vector2 lastMovement = new Vector2(1, 0);

	//Layers
	private int floorLayer;
	private int birdLayer;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		playerRend = GetComponent<SpriteRenderer>();

		floorLayer = LayerMask.GetMask("Floor");
		birdLayer = LayerMask.GetMask("Parrot");
	}

	void Update()
	{
		movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (movementVector != new Vector2(0, 0))
		{
			lastMovement = movementVector;
		}

		if (Input.GetKeyDown(KeyCode.Space) && canJump && !isDashing)
		{
			Jumping();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
		{
			Dashing();
		}

		if(canDash)
		{
			playerRend.color = Color.magenta;
		}
		else
		{
			playerRend.color = Color.white;
		}
	}

	void FixedUpdate()
	{
		if (!isDashing)
		{
			Walking();
			jumpCheck();
		}
		
	}

	void Walking()
	{
		rb.velocity = new Vector2(movementVector.x * settings.walkSpeed, rb.velocity.y);
	}

	void Jumping()
	{
		canJump = false;

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

	void jumpCheck()
	{
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (GetComponent<CapsuleCollider2D>().size.y / 2));

		RaycastHit2D floorHit = Physics2D.Raycast(rayOrigin, Vector2.down, settings.canJumpDistance, floorLayer);

		RaycastHit2D birdHit = Physics2D.Raycast(rayOrigin, Vector2.down, settings.canJumpDistance, birdLayer);

		if (floorHit.collider != null)
		{
			canJump = true;
			canDash = true; 
		}
		else if (birdHit.collider != null)
		{
			canJump = true;
		}
		else
		{
			canJump = false; 
		}
	}

	IEnumerator OnDash()
	{
		yield return new WaitForSeconds(settings.dashTime);

		rb.gravityScale = 3f; 

		isDashing = false;
	}
}