using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Camera
{
	public class CameraFollow : MonoBehaviour
	{
		[SerializeField] private Transform lookAt;
		[SerializeField] private float smoothTime = 2f;
		[SerializeField] private Vector3 offsetPosition = new Vector3(0f, 0f, -10f);

		private Vector3 targetPosition;
		private Vector3 velocity;

		private void LateUpdate ()
		{
			MoveCamera ();
		}

		private void MoveCamera()
		{
			targetPosition = lookAt.TransformPoint(offsetPosition);
			this.transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);
		}
	}
}
