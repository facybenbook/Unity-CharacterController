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

		float gravity = -20f;
		[SerializeField] float moveSpeed = 6f;
		#endregion

		#region Unity Methods
		void Start () 
		{
			controller = GetComponent<Controller2D> ();
		}

		private void Update() 
		{
			input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			velocity.x = input.x * moveSpeed * Time.deltaTime;
			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);
		}
		#endregion
	}
}

