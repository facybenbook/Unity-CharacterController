using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player.Controller
{
	[RequireComponent(typeof(Controller2D))]
	public class Player : MonoBehaviour 
	{
		#region References
		Controller2D controller;
		#endregion

		#region Variables
		Vector3 velocity;
		Vector2 input;

		[SerializeField] float moveSpeed = 6f;
		[SerializeField] float jumpHeight = 4f;
		[SerializeField] float timeToJumpApex = 0.4f;

		[SerializeField] float accelerationTimeAirborne = 0.2f;
		[SerializeField] float accelerationTimeGrounded = 0.1f;

		float gravity;
		float jumpVelocity;
		float velocityXSmoothing;
		#endregion

		#region Unity Methods
		void Start () 
		{
			controller = GetComponent<Controller2D> ();
			
			gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
			jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);

			Debug.Log("Gravity is" + gravity);
			Debug.Log("Jump Velocity is " + jumpVelocity);
		}

		private void Update() 
		{
			if (controller.collisionInfo.above || controller.collisionInfo.below)
			{
				velocity.y = 0;
			}

			input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			if (Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below)
			{
				velocity.y = jumpVelocity;
			}
			
			float targetVelocityX = input.x * moveSpeed;
			float targetAcceleration = (controller.collisionInfo.below) ? accelerationTimeGrounded : accelerationTimeAirborne;

			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, targetAcceleration);
			velocity.y += gravity * Time.deltaTime;

			controller.Move(velocity * Time.deltaTime);
		}
		#endregion
	}
}

