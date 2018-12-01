using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS.Configurations
{
	[CreateAssetMenu(menuName = "Weapon/WeaponProperties")]
	public class WeaponProperties : ScriptableObject
	{
		public Vector3 righHandPosition;
		public Vector3 righHandRotation;
		public GameObject weaponPrefab;
		public int damage;
	}
}