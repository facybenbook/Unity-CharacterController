using TPS.CameraController;
using TPS.Configurations;
using UnityEngine;

namespace TPS.CharacterController
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CharacterStatus))]
	public class CharacterMovement : MonoBehaviour
	{
		#region References
		public Transform cameraHolderTransform;
		public CharacterStatus characterStatus;
		public LayerMask groundMask;
		#endregion References

		#region Variables
		public float turnSpeed = 5f;

		[HideInInspector]
		public bool isRun;
		[HideInInspector]
		public float verticalInput;
		[HideInInspector]
		public float horizontalInput;
		[HideInInspector]
		public float movingSpeedFoAnimation;

		private float groundChekerRadius = 0.7f;

		private Vector3 rotationDirection;
		private Vector3 moveDirection;
		#endregion Variables

		
		public void RunUpdate()
		{
			isRun = Input.GetButtonDown (Statics.Shift);
			if (isRun)
			{
				characterStatus.isSprint = !characterStatus.isSprint;
			}
		}

		public void MoveUpdate ()
		{
			// Get Input
			verticalInput = Input.GetAxis (Statics.Vertical);
			horizontalInput = Input.GetAxis (Statics.Horizontal);
			// Get an absolute value for animation
			movingSpeedFoAnimation = Mathf.Clamp01 (Mathf.Abs (verticalInput) + Mathf.Abs (horizontalInput));

			// Move forward to the camera view
			Vector3 targetMoveDirection = cameraHolderTransform.forward * verticalInput;
			// Move to left or right of the camera view
			targetMoveDirection += cameraHolderTransform.right * horizontalInput;
			targetMoveDirection.Normalize ();

			moveDirection = targetMoveDirection;
			// Always look at camera forward position
			rotationDirection = cameraHolderTransform.forward;

			RotationNormal ();
			characterStatus.isGround = IsGround ();
		}

		/// <summary>
		/// Rotation of a the character by using the camera
		/// </summary>
		public void RotationNormal()
		{
			// If the character is not in the aiming state, 
			// don't rotate him when the camera is rotating 
			if (!characterStatus.isAiming)
			{
				// Look forward when camera is rotating
				rotationDirection = moveDirection;
			}

			Vector3 targetRotationDirection = rotationDirection;
			targetRotationDirection.y = 0; // don't fly

			if (targetRotationDirection == Vector3.zero)
			{
				targetRotationDirection = this.transform.forward;
			}

			// Get rotation value where the character must to rotate
			Quaternion lookDirection = Quaternion.LookRotation (targetRotationDirection);
			// Spherically interpolate to the look direction
			Quaternion targetRotation = Quaternion.Slerp (this.transform.rotation, lookDirection, turnSpeed * Time.deltaTime);
			this.transform.rotation = targetRotation;
		}

		public bool IsGround()
		{
			Vector3 origin = this.transform.position;
			origin.y += 0.6f;
			Vector3 directionBottom = -Vector3.up;

			// Get RaycastHit Information 
			RaycastHit hit;
			if (Physics.Raycast(origin, directionBottom, out hit, groundChekerRadius))
			{
				this.transform.position = hit.point;
				return true;
			}

			return false;
		}
	}
}