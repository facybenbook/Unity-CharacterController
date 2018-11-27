using System.Collections;
using System.Collections.Generic;
using TPS.Configurations;
using UnityEngine;

namespace TPS.WeaponController
{
	public class Weapon : MonoBehaviour 
	{
		#region References
		public WeaponProperties weaponProperties;
		public Transform shootPoint;
		public Transform targetLook;

		public GameObject cameraMain;
		public GameObject decal;
		public GameObject bullet;
		#endregion

		void Update () 
		{
			shootPoint.LookAt(targetLook);
			Vector3 origin = shootPoint.position;
			Vector3 direction = targetLook.position;

			RaycastHit hit;
			// decal.SetActive(false);
			Debug.DrawLine(origin, direction, Color.red);
			Debug.DrawLine(cameraMain.transform.position, direction, Color.green);

			if (Physics.Linecast(origin, direction, out hit))
			{
				// decal.SetActive(true);
				Vector3 moveDecalOverTarget = hit.normal * 0.01f;
				decal.transform.position = hit.point + moveDecalOverTarget;
				decal.transform.rotation = Quaternion.LookRotation(-hit.normal);

			}
		}

		public void Shoot()
		{
			GameObject bulletClone = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
		}
	}
}
