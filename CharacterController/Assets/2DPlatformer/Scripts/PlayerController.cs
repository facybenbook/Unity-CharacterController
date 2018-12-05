using System.Collections;
using System.Collections.Generic;
using Platformer.Managers;
using UnityEngine;

namespace Platformer.Player.Controller
{
	[RequireComponent (typeof (CharacterController))]
	public class PlayerController : MonoBehaviour
	{
		#region References
		private CharacterController characterController;
		#endregion

		#region Input
		private bool spacebarIsPressed;
		private float horizontalVelocity;
		private float verticalVelocity;
		#endregion

		#region Variables
		private Vector3 moveVector;
		private Vector3 lastMotion; // save an old moving direction trajectory for the double jump

		[SerializeField] private float moveSpeed = 7.0f;
		[SerializeField] [Range (0, 1)] private float moveSmoothJumpSpeed = 0.5f;
		[SerializeField] private float gravity = 25.0f;
		[SerializeField] private float jumpForce = 10f;
		[SerializeField] private float doubleJumpForce = 15f;
		private const float skinWidth = 0.05f;
		private bool canDoDoubleJump;
		#endregion

		#region Unity Methods
		void Start ()
		{
			characterController = GetComponent<CharacterController> ();
		}

		void Update ()
		{
			IsControllerGrounded ();
			InputHandler ();
			Movement ();
		}

		void OnControllerColliderHit (ControllerColliderHit hit)
		{
			if (characterController.collisionFlags == CollisionFlags.Sides)
			{
				if (spacebarIsPressed)
				{
					Debug.DrawRay (hit.point, hit.normal, Color.red, 1.0f);
					moveVector = hit.normal * moveSpeed;
					moveVector.z = 0f;
					verticalVelocity = doubleJumpForce;
					canDoDoubleJump = true;
				}
			}

			switch (hit.gameObject.tag)
			{
				case Statics.Coin:
					// TODO: collect coin
					Destroy (hit.gameObject);
					break;
				default:
					break;
			}
		}
		#endregion

		#region Player Controller Methods

		private void InputHandler ()
		{
			spacebarIsPressed = Input.GetKeyDown (KeyCode.Space);
			horizontalVelocity = Input.GetAxis (Statics.Horizontal) * moveSpeed;
		}

		private void Movement ()
		{
			if (IsControllerGrounded ())
			{
				Debug.Log ("Is Grounded");
				verticalVelocity = 0;

				if (spacebarIsPressed)
				{
					Debug.Log ("Is Jump");
					verticalVelocity = jumpForce;
					canDoDoubleJump = true;
				}

				moveVector.x = horizontalVelocity;
			}
			else
			{
				if (spacebarIsPressed && canDoDoubleJump)
				{
					Debug.Log ("Is Double Jump");
					canDoDoubleJump = false;
					verticalVelocity = doubleJumpForce;
				}

				Debug.Log ("Move Down");
				verticalVelocity -= gravity * Time.deltaTime;
				Debug.Log ("Is Moving");
				// Continue to move by previous moving trajectory 
				moveVector.x = lastMotion.x;
			}

			moveVector.y = verticalVelocity;
			moveVector.z = 0f;

			characterController.Move (moveVector * Time.deltaTime);
			lastMotion = moveVector;
		}

		private bool IsControllerGrounded ()
		{
			Vector3 leftRayStartPos;
			Vector3 rightRayStartPos;

			leftRayStartPos = characterController.bounds.center;
			rightRayStartPos = characterController.bounds.center;

			leftRayStartPos.x -= characterController.bounds.extents.x;
			rightRayStartPos.x += characterController.bounds.extents.x;

			Debug.DrawRay (leftRayStartPos, -Vector3.up, Color.red);
			Debug.DrawRay (rightRayStartPos, -Vector3.up, Color.red);

			float distanceToGround = characterController.height * 0.5f + skinWidth;
			if (Physics.Raycast (leftRayStartPos, Vector3.down, distanceToGround))
			{
				return true;
			}
			return false;
		}
		#endregion
	}
}

