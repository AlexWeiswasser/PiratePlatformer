using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

	//Components
	private Rigidbody2D rb;

	//Variables
	public float walkSpeed = 7f;
	public float jumpForce = 10f;
	public float canJumpDistance = 1.1f;

	//Booleans
	bool canJump = true;

	//Vectors
	Vector2 movementVector = new Vector2(0, 0);

	//Layers
	int floorLayer;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		floorLayer = LayerMask.GetMask("Floor");
	}

	void Update()
	{
		movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (Input.GetKeyDown("w") && canJump)
		{
			Jumping();
		}
	}

	void FixedUpdate()
	{
		Walking();
		jumpCheck();
	}

	void Walking()
	{
		rb.velocity = new Vector2(movementVector.x * walkSpeed, rb.velocity.y);
	}

	void Jumping()
	{
		canJump = false;

		rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

	void jumpCheck()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, canJumpDistance, floorLayer);

		if (hit.collider != null)
		{
			canJump = true;
		}
		else
		{
			canJump = false;
		}
	}

}