using UnityEngine;

[CreateAssetMenu(fileName = "Player Controller Settings", menuName = "Platformer/Player Settings")]

public class PlayerControllerSettings : ScriptableObject
{
	public float walkSpeed = 7f;
	public float jumpForce = 10f;
	public float canJumpDistance = .1f;
	public float dashForce = 13f;
	public float dashTime = .15f;
}
