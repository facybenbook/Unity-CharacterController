using UnityEngine;
using TPS.Configurations;
using TPS.Managers;

namespace TPS.CameraController
{
	public class CameraHandler : MonoBehaviour
	{
		#region References
		private Transform cameraHolderTransform;
		public Transform mainCameraTransform;
		public Transform cameraPivot;
		public Transform character;


		public CharacterStatus characterStatus;
		public CameraConfig cameraConfig;
		public Transform targetLook;
		#endregion

		#region Variables
		public float targetLookDistanceToSky = 200f;
		public float targetLookDistanceRaycast = 2000f;
		public float targetLookSpeed = 40f;
		public float targetLookSpeedToSky = 5f;

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


		private void FixedTick ()
		{
			deltaT = Time.deltaTime;

			HandlePosition ();
			HandleRotation ();

			Vector3 targetPosition = Vector3.Lerp (cameraHolderTransform.position, character.position, cameraConfig.cameraHolderMoveSpeed);
			cameraHolderTransform.position = targetPosition;
			
			TargetLook();
		}

		private void TargetLook()
		{
			Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward * targetLookDistanceRaycast);
			
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				targetLook.position = Vector3.Lerp(targetLook.position, hit.point, Time.deltaTime * targetLookSpeed);
			}
			else
			{
				targetLook.position = Vector3.Lerp(targetLook.position, targetLook.forward * targetLookDistanceToSky, Time.deltaTime * targetLookSpeedToSky);
			}
			
		}

		private void HandlePosition()
		{
			// Set the Camera position
			float targetX = cameraConfig.normalXPos;
			float targetY = cameraConfig.normalYPos;
			float targetZ = cameraConfig.normalZPos;

			// Switch aim and default camera state
			if (characterStatus.isAiming)
			{
				targetX = cameraConfig.aimXPos;
				targetY = cameraConfig.aimYPos;
				targetZ = cameraConfig.aimZPos;
			}

			// Go to left shoulder view
			if (cameraConfig.lookFromLeft)
			{
				targetX = -targetX;
			}


			// X and Y position of the camera's pivot
			Vector3 newPivotPositon = cameraPivot.localPosition;
			newPivotPositon.x = targetX;
			newPivotPositon.y = targetY;
			cameraPivot.localPosition = newPivotPositon;

			// Z position of the camera
			Vector3 newCameraPosition = mainCameraTransform.localPosition;
			newCameraPosition.z = targetZ;

			float cameraDeltaTime = deltaT * cameraConfig.pivotSpeed;

			// Follow to character
			cameraPivot.localPosition = Vector3.Lerp (cameraPivot.localPosition, newCameraPosition, cameraDeltaTime);
			mainCameraTransform.localPosition = Vector3.Lerp (mainCameraTransform.localPosition, newCameraPosition, cameraDeltaTime);
		}

		private void HandleRotation()
		{
			// Get mouse position
			mouseX = Input.GetAxis (Statics.Mouse_xInput);
			mouseY = Input.GetAxis (Statics.Mouse_yInput);

			// Add additional smoothing if necessary or use default
			AddCameraSmoothing ();

			// Get angle of the camera view
			X_lookAngle += smoothX * cameraConfig.XRotationSpeed;
			Y_lookAngle -= smoothY * cameraConfig.YRotationSpeed;

			if (!characterStatus.isAiming)
			{
				Y_lookAngle = Mathf.Clamp (Y_lookAngle, cameraConfig.minYViewAngle, cameraConfig.maxYViewAngle);
			}
			else
			{
				Y_lookAngle = Mathf.Clamp (Y_lookAngle, cameraConfig.minYAimingViewAngle, cameraConfig.maxYAimingViewAngle);
			}

			// Switch between aim and default camera state
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

		private void AddCameraSmoothing()
		{
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
		}
		#endregion
	}
}