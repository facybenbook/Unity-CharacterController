using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS.UI
{
	public class HitMarker : MonoBehaviour 
	{
		#region References
		public GameObject hitMarkerPrefab;
		public AudioSource audioSource;
		public AudioClip audioClip;
		#endregion

		#region Variables
		private float timeLive;
		public float maxTimeLive;
		#endregion

		#region Unity Methods	

		private void Awake() {
			audioSource = GetComponent<AudioSource>();
			hitMarkerPrefab.SetActive(false);
		}

		void Update () 
		{
			if (timeLive == maxTimeLive)
			{
				hitMarkerPrefab.SetActive(true);
				audioSource.PlayOneShot(audioClip);
			}
			else if (timeLive == 0)
			{
				hitMarkerPrefab.SetActive(false);
			}

			timeLive -= Time.deltaTime;
			timeLive = Mathf.Clamp(timeLive, 0, maxTimeLive);
		}
		#endregion

		public void ShowHitMarker ()
		{
			timeLive = maxTimeLive;
		}
	}
}
