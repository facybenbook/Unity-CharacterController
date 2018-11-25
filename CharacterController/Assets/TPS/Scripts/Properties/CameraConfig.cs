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
		public float defaultSmooth = 0.1f;

		[Header("Rotation Speed")]
		public float pivotSpeed;
		public float YRotationSpeed;
		public float XRotationSpeed;
		public float shiftToTheLeftSpeed;

		[Header("Angle of the camera view")]
		public float minYViewAngle;
		public float maxYViewAngle;

		[Header("Camera Position")]
		public float normalZPos;
		public float normalXPos;
		public float normalYPos;
		public bool lookFromLeft;

		[Header("Camera Aim position")]
		public float aimYPos;
		public float aimXPos;
		public float aimZPos;
	}
}