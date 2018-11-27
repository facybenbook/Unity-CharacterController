using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS.WeaponController
{
	public class Bullet : MonoBehaviour 
	{
		public GameObject decalPrefab;
		public float decalDeathTime = 5f;
		public float bulletDeathTime = 0.5f;
		public float speed;
		Vector3 previousBulletPosition;

		private void Start() 
		{
			previousBulletPosition = transform.position;
		}

		void Update () 
		{
			MoveBullet ();
			BulletLinecast ();
		}

		void MoveBullet()
		{
			this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		void BulletLinecast()
		{
			RaycastHit hit;

			Debug.DrawLine(previousBulletPosition, transform.position);
			if (Physics.Linecast(previousBulletPosition, transform.position, out hit))
			{
				Debug.Log (hit.transform.name);

				GameObject decal = Instantiate<GameObject>(decalPrefab);
				Vector3 moveDecalOverTarget = hit.normal * 0.01f;
				decal.transform.position = hit.point + moveDecalOverTarget;
				decal.transform.rotation = Quaternion.LookRotation(-hit.normal);
				
				Destroy(this.gameObject);				
				Destroy (decal, decalDeathTime);
			}
			previousBulletPosition = transform.position;
			Destroy(this.gameObject, bulletDeathTime);
		}
	}
}
