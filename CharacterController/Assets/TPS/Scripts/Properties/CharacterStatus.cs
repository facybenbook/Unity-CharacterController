using UnityEngine;

namespace TPS.CameraController
{
	[CreateAssetMenu(menuName = "Character/CharacterStatus")]
	public class CharacterStatus : ScriptableObject
	{
		public bool isAiming;
		public bool isSprint;
		public bool isGround;
	}
}