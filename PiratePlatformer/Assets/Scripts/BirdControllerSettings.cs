using UnityEngine;

[CreateAssetMenu(fileName = "Bird Controller Settings", menuName = "Platformer/Bird Settings")]

public class BirdControllerSettings : ScriptableObject
{
	public float birdLerp = 5f;
	public float baseBirdHealth = 1f;
	public float birdHealCD = 1f;
	public float platformCheckHeight = -.90f;
	public float birdHealingRate = .25f;
}
