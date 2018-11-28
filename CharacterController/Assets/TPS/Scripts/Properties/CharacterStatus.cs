using UnityEngine;

namespace TPS.Configurations
{
	[CreateAssetMenu(menuName = "Character/CharacterStatus")]
	public class CharacterStatus : ScriptableObject
	{
		[Header("Character States")]
		public bool isAiming;
		public bool isMoving;
		public bool isSprint;
		public bool isGround;
		public bool isWeapon;
	}
}