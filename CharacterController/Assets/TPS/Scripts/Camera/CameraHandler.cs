using UnityEngine;

namespace TPS.CameraController
{
	public class CameraHandler : MonoBehaviour
	{
		#region References
		public Transform cameraTransform;
		public Transform cameraPivot;
		public Transform character;

		private Transform cameraHolderTransform;

		public CharacterStatus characterStatus;
		public CameraConfig cameraConfig;
		#endregion

		#region Variables
		private bool leftPivot;
		private float deltaT;

		private float mouseX;
		private float mouseY;

		private float smoothX;
		private float smoothY;
		private float smooth_XVelocity;
		private float smooth_YVelocity;

		private float Y_lookAngle;
		private float X_lookAngle;
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
			deltaT = Time.deltaTime;

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

			Vector3 newPivotPositon = cameraPivot.localPosition;
			newPivotPositon.x = targetX;
			newPivotPositon.y = targetY;
			cameraPivot.localPosition = newPivotPositon;

			Vector3 newCameraPosition = cameraTransform.localPosition;
			newCameraPosition.z = targetZ;

			float cameraDeltaTime = deltaT * cameraConfig.pivotSpeed;

			cameraPivot.localPosition = Vector3.Lerp (cameraPivot.localPosition, newCameraPosition, cameraDeltaTime);
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
				smoothX = cameraConfig.defaultSmooth;
			}

			if (cameraConfig.turnSmoothY > 0)
			{
				smoothY = Mathf.SmoothDamp (smoothY, mouseY, ref smooth_YVelocity, cameraConfig.turnSmoothY);
			}
			else
			{
				smoothY = cameraConfig.defaultSmooth;
			}

			X_lookAngle += smoothX * cameraConfig.XRotationSpeed;
			Y_lookAngle -= smoothY * cameraConfig.YRotationSpeed;
			Y_lookAngle = Mathf.Clamp (Y_lookAngle, cameraConfig.minYViewAngle, cameraConfig.maxYViewAngle);

			if (characterStatus.isAiming)
			{
				cameraHolderTransform.rotation = Quaternion.Euler(0f, X_lookAngle, 0f);
				cameraPivot.localRotation = Quaternion.Euler (Y_lookAngle, 0f, 0f);
			}
			else
			{
				cameraHolderTransform.rotation = Quaternion.Euler (Y_lookAngle, X_lookAngle, 0f);
			}
		}
		#endregion
	}
}