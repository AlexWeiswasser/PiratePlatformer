using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	//GameObjects
	[SerializeField] private Transform player;

	//Variables
	public float cameraSpeed = 5f;

	// Update is called once per frame
	void LateUpdate()
	{
		if (player != null)
		{
			float newPos = Mathf.Lerp(transform.position.x, player.position.x, Time.deltaTime * cameraSpeed);

			transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
		}
	}
}
