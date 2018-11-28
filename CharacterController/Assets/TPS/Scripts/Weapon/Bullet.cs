using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS.WeaponController
{
	public class Bullet : MonoBehaviour 
	{
		#region References
		[Header("Decal Prefabs")]
		public GameObject decalPrefab;

		[Header("Particle Effects")]
		public GameObject metalHitEffect;
		public GameObject sandHitEffect;			
		public GameObject stoneHitEffect;
		public GameObject woodHitEffect;
		public GameObject[] meatHitEffects;
		#endregion

		#region Variables
		[Header("Weapon parameters")]
		public float decalDeathTime = 10f;
		public float bulletDeathTime = 10f;
		public float speed;
		Vector3 previousBulletPosition;
		#endregion

		#region Unity Methods
		private void Start() 
		{
			previousBulletPosition = transform.position;
			Destroy(gameObject, bulletDeathTime);
		}

		void Update () 
		{
			MoveBullet ();
			BulletLinecast ();
		}
		#endregion

		#region Bullet Methods

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
				if (hit.collider.sharedMaterial != null)
				{
					string materialName = hit.collider.sharedMaterial.name;
					switch (materialName)
					{
						case "Metal":
							SpawnDecal(hit, metalHitEffect);
							break;
						case "Sand":
							SpawnDecal(hit, sandHitEffect);
							break;
						case "Stone":
							SpawnDecal(hit, stoneHitEffect);
							break;
						case "Wood":
							SpawnDecal(hit, woodHitEffect);
							break;
						case "Meat":
							SpawnDecal(hit, meatHitEffects[Random.Range(0, meatHitEffects.Length)]);
							break;
					}
				}
				Destroy(this.gameObject);				
			}
			previousBulletPosition = transform.position;
		}

		void SpawnDecal (RaycastHit hit, GameObject prefab)
		{
			GameObject spawnDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
			spawnDecal.transform.SetParent(hit.collider.transform);
			Destroy(spawnDecal.gameObject, decalDeathTime);
		}

		#endregion
	}
}
