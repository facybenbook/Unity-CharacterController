using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPS.UI
{
	public class Crosshair : MonoBehaviour 
	{
		#region References
		public CrosshairElements[] crosshairElements;
		public CharacterController.CharacterMovement character;
		#endregion

		#region Variables
		[Header("Spread Parameters")]
		public float targetSpreadWidth;
		public float targetSpreadMovingWidth;
		public float speedSpread;

		private float time;
		private float currentSpreadWidth;
		#endregion

		#region Unity Methods
		private void Start() 
		{
			time = 0.005f * speedSpread;
		}

		void Update () 
		{
			CrosshairUpdate ();
			CrosshairMoving ();
		}
		#endregion

		#region Crosshair Methods
		public void CrosshairUpdate()
		{
			currentSpreadWidth = Mathf.Lerp(currentSpreadWidth, targetSpreadWidth, time);

			for (int i = 0; i < crosshairElements.Length; i++)
			{
				CrosshairElements element = crosshairElements[i];
				element.transform.anchoredPosition = element.position * currentSpreadWidth;
			}
		}

		public void CrosshairMoving()
		{
			if (character.absoluteSpeed > 0)
			{
				currentSpreadWidth = targetSpreadWidth * (targetSpreadMovingWidth + character.absoluteSpeed);
			}
			else
			{
				currentSpreadWidth = targetSpreadWidth;
			}
		}
		#endregion

		[System.Serializable]
		public class CrosshairElements
		{
			public RectTransform transform;
			public Vector2 position;
		}
	}
}

