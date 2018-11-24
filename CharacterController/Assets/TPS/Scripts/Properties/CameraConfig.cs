using UnityEngine;

namespace TPS.CameraController
{
	[CreateAssetMenu(menuName = "Camera/CameraConfig")]
	public class CameraConfig : ScriptableObject
	{
		[Header ("Camera Holder moving speed")]
		public float cameraHolderMoveSpeed;

		[Header("Camera Turn Smoothing")]
		public float turnSmoothX;
		public float turnSmoothY;

		[Header("Rotation Speed")]
		public float pivotSpeed;
		public float YRotationSpeed;
		public float XRotationspeed;

		[Header("Angle of the camera view")]
		public float minXViewAngle;
		public float maxXViewAngle;

		[Header("Camera Position")]
		public float normalZPos;
		public float normalXPos;
		public float normalYPos;

		[Header("Camera Aim position")]
		public float aimZPos;
		public float aimXPos;
	}
}