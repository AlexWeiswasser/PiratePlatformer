using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotCode : MonoBehaviour
{
	//Gameobjects
	[SerializeField] private GameObject Player;

	//Variables
	float birdHealth; 

	//Componenets
	private Collider2D birdCol;
	private SpriteRenderer birdRend;
	public BirdControllerSettings settings;

	//Vectors
	private Vector2 mousePos;

	//Booleans
	private bool playerOnTop = false;
	private bool birdHeal = true;
	private bool isMouseDown = false; 

	void Start()
	{
		birdCol = GetComponent<Collider2D>();
		birdRend = GetComponent<SpriteRenderer>();

		birdHealth = settings.baseBirdHealth;
	}

	private void Update()
	{
		Cursor.visible = false;

		BirdCalculations();

		if (Input.GetMouseButton(0))
		{
			isMouseDown = true;

			birdRend.color = Color.gray;
		}
		else
		{
			isMouseDown = false;

			BirdColor();
		}
	}

	void LateUpdate()
	{
		mousePos = GetMousePosition();

		if (!isMouseDown)
		{
			transform.position = Vector3.Lerp(transform.position, mousePos, Time.deltaTime * settings.birdLerp);
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
				birdHealth += settings.birdHealingRate * Time.deltaTime;
			}
		}

		if (birdHealth == 0)
		{
			birdCol.enabled = false;
			StartCoroutine(BirdHealingCooldown());
			birdHeal = false;
			playerOnTop = false;
		}
		else
		{
			birdCol.enabled = true;
		}
	}

	void BirdColor()
	{
		Color newColor = Color.Lerp(Color.red, Color.white, birdHealth);

		birdRend.color = newColor;
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isMouseDown && collision.gameObject == Player)
		{
			foreach (ContactPoint2D contact in collision.contacts)
			{
				if (contact.normal.y < settings.platformCheckHeight) 
				{
					birdHealth -= 0.15f;
					playerOnTop = true;
				}
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (!isMouseDown && collision.gameObject == Player)
		{
			foreach (ContactPoint2D contact in collision.contacts)
			{
				if (contact.normal.y < settings.platformCheckHeight)
				{
					playerOnTop = true;
					birdHealth -= 1f * Time.deltaTime;
					return; 
				}
			}
			playerOnTop = false; 
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (!isMouseDown && collision.gameObject == Player)
		{
			if (Player.transform.position.y >= transform.position.y - .1)
			{
				StartCoroutine(BirdHealingCooldown());
				birdHeal = false;
				playerOnTop = false;
			}
		}
	}

	IEnumerator BirdHealingCooldown()
	{
		yield return new WaitForSeconds(settings.birdHealCD);
		birdHeal = true;
	}
}
