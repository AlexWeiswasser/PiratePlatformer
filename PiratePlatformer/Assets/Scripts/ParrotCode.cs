using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotCode : MonoBehaviour
{
	//Gameobjects
	[SerializeField] GameObject Player;

	//Componenets
	Collider2D birdCol;

	//Variables
	public float birdLerp = 2f;
	public float birdHealth = 1f;
	public float birdHealCD = 1f;

	//Vectors
	Vector2 mousePos;

	//Booleans
	bool playerOnTop = false;
	bool birdHeal = true;
	bool isMouseDown = false; 

	void Start()
	{
		Cursor.visible = false;

		birdCol = GetComponent<Collider2D>();
	}

	private void Update()
	{
		BirdCalculations();

		if (Input.GetMouseButton(0))
		{
			isMouseDown = true;
		}
		else
		{
			isMouseDown = false;
		}
	}

	void FixedUpdate()
	{
		mousePos = GetMousePosition();

		if (!isMouseDown)
		{
			transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime * birdLerp);
		}
	}

	Vector2 GetMousePosition()
	{
		Vector3 mouseScreenPosition = Input.mousePosition;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
		mouseWorldPosition.z = 0f;  
		Vector2 mouse2DPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

		return mouse2DPosition;
	}

	void BirdCalculations()
	{
		if(birdHealth > 1)
		{
			birdHealth = 1;
		}
		if(birdHealth < 0)
		{
			birdHealth = 0;
		}

		if (!isMouseDown)
		{
			if (!playerOnTop && birdHealth < 1f && birdHeal)
			{
				birdHealth += .15f * Time.deltaTime;
			}
		}

		if (birdHealth == 0)
		{
			birdCol.enabled = false;
		}
		else
		{
			birdCol.enabled = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isMouseDown)
		{
			if (collision.gameObject == Player)
			{
				birdHealth -= .25f;
			}
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (!isMouseDown)
		{
			if (collision.gameObject == Player)
			{
				playerOnTop = true;
				birdHealth -= 1f * Time.deltaTime;
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject == Player)
		{
			StartCoroutine(BirdHealingCooldown());
			birdHeal = false;
			playerOnTop = false;
		}
	}

	IEnumerator BirdHealingCooldown()
	{
		yield return new WaitForSeconds(birdHealCD);
		birdHeal = true;
	}
}
