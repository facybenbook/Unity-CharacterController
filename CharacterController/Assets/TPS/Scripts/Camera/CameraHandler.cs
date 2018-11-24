using UnityEngine;

namespace TPS.CameraController
{
	public class CameraHandler : MonoBehaviour
	{
		#region References
		public Transform cameraTransform;
		public Transform pivot;
		public Transform character;

		private Transform cameraHolderTransform;

		public CharacterStatus characterStatus;
		public CameraConfig cameraConfig;
		#endregion

		#region Variables
		public bool leftPivot;
		public float delta;

		public float mouseX;
		public float mouseY;

		private float defaultSmooth = 0.1f;
		public float smoothX;
		public float smoothY;
		public float smooth_XVelocity;
		public float smooth_YVelocity;

		public float Y_lookAngle;
		public float X_lookAngle;
		#endregion

		#region Unity Methods
		private void Awake ()
		{
			cameraHolderTransform = this.transform;
		}

		private void FixedUpdate ()
		{
			FixedTick ();
		}
		#endregion

		#region CameraHandler Methods
		void FixedTick ()
		{
			delta = Time.deltaTime;

			HandlePosition ();
			HandleRotation ();

			Vector3 targetPosition = Vector3.Lerp (cameraHolderTransform.position, character.position, cameraConfig.cameraHolderMoveSpeed);
			cameraHolderTransform.position = targetPosition;
		}

		void HandlePosition()
		{
			float targetX = cameraConfig.normalXPos;
			float targetY = cameraConfig.normalYPos;
			float targetZ = cameraConfig.normalZPos;

			if (characterStatus.isAiming)
			{
				targetX = cameraConfig.aimXPos;
				targetZ = cameraConfig.aimZPos;
			}

			if (leftPivot)
			{
				targetX = -targetX;
			}

			Vector3 newPivotPositon = pivot.localPosition;
			newPivotPositon.x = targetX;
			newPivotPositon.y = targetY;
			pivot.localPosition = newPivotPositon;

			Vector3 newCameraPosition = cameraTransform.localPosition;
			newCameraPosition.z = targetZ;

			float cameraDeltaTime = delta * cameraConfig.pivotSpeed;

			pivot.localPosition = Vector3.Lerp (pivot.localPosition, newCameraPosition, cameraDeltaTime);
			cameraTransform.localPosition = Vector3.Lerp (cameraTransform.localPosition, newCameraPosition, cameraDeltaTime);
		}

		void HandleRotation()
		{
			mouseX = Input.GetAxis ("Mouse X");
			mouseY = Input.GetAxis ("Mouse Y");

			if (cameraConfig.turnSmoothX > 0)
			{
				smoothX = Mathf.SmoothDamp (smoothX, mouseX, ref smooth_XVelocity, cameraConfig.turnSmoothX);
			}
			else
			{
				smoothX = defaultSmooth;
			}

			if (cameraConfig.turnSmoothY > 0)
			{
				smoothY = Mathf.SmoothDamp (smoothY, mouseY, ref smooth_YVelocity, cameraConfig.turnSmoothY);
			}
			else
			{
				smoothY = defaultSmooth;
			}


			Y_lookAngle += smoothX * cameraConfig.YRotationSpeed;
			Quaternion targetRotation = Quaternion.Euler (0, Y_lookAngle, 0);
			cameraHolderTransform.rotation = targetRotation;

			X_lookAngle -= smoothY * cameraConfig.YRotationSpeed;
			X_lookAngle = Mathf.Clamp (X_lookAngle, cameraConfig.minXViewAngle, cameraConfig.maxXViewAngle);
			pivot.localRotation = Quaternion.Euler (X_lookAngle, 0, 0);
		}
		#endregion
	}
}