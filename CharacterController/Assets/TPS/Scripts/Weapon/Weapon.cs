using System.Collections;
using System.Collections.Generic;
using TPS.Configurations;
using UnityEngine;

namespace TPS.WeaponController
{
	public class Weapon : MonoBehaviour 
	{
		#region References
		[Header("Weapon Properties")]
		public WeaponProperties weaponProperties;

		[Header("Camera")]
		public GameObject cameraMain;
		public Transform targetLook;

		[Header("Effects")]
		public ParticleSystem muzzleFlash;

		[Header("Audio")]
		private AudioSource audioSource;
		public AudioClip shootClip;

		[Header("Shooting Elements")]
		public Transform shootPoint;
		public GameObject decal;
		public GameObject bullet;

		[Header("Bullet Shell Parameters")]
		public GameObject shell;
		public Transform shellPosition;
		public float shellFlightRadiusMin = 80f;
		public float shellFlightRadiusMax = 120f;
		public float shellLifeTime = 10f;
		#endregion

		private void Awake() 
		{
			audioSource = GetComponent<AudioSource> ();
		}

		private void Update () 
		{
			shootPoint.LookAt(targetLook);
			Vector3 origin = shootPoint.position;
			Vector3 direction = targetLook.position;

			RaycastHit hit;
			// decal.SetActive(false);
			Debug.DrawLine(origin, direction, Color.red);
			Debug.DrawLine(cameraMain.transform.position, direction, Color.green);

			// if (Physics.Linecast(origin, direction, out hit))
			// {
			// 	// decal.SetActive(true);
			// 	Vector3 moveDecalOverTarget = hit.normal * 0.01f;
			// 	decal.transform.position = hit.point + moveDecalOverTarget;
			// 	decal.transform.rotation = Quaternion.LookRotation(-hit.normal);

			// }
		}

		public void Shoot()
		{
			GameObject bulletClone = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
			audioSource.PlayOneShot(shootClip);
			muzzleFlash.Play();
			
			AddBulletShell();
		}

		private void AddBulletShell()
		{
			GameObject shellClone = Instantiate(shell);
			shellClone.transform.position = shellPosition.position;
			shellClone.transform.rotation = shellPosition.rotation;
			shellClone.transform.parent = null;

			shellClone.GetComponent<Rigidbody>().AddForce(-shellClone.transform.forward * Random.Range(shellFlightRadiusMin, shellFlightRadiusMin));

			Destroy(shellClone.gameObject, shellLifeTime);
		}
	}
}
