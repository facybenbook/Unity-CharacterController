using UnityEngine;

namespace CharacterController
{
	[DisallowMultipleComponent]
	[RequireComponent (typeof (Animator))]
	[RequireComponent (typeof (Rigidbody))]
	[RequireComponent (typeof (CapsuleCollider))]
	public class PlayerController : MonoBehaviour
	{
		#region References

		private Animator animator;

		#endregion

		#region Variables

		float horizontal;
		float vertical;
		float speed;
		float jump;

		bool isWalking;
		bool isMoving;

		#endregion

		#region Unity Methods
		private void Awake ()
		{
			animator = GetComponent<Animator> ();
		}

		private void Update ()
		{
			Movement ();
		}
		#endregion

		#region Move Controller
		private void Movement()
		{
			horizontal = Input.GetAxis ("Horizontal");
			vertical = Input.GetAxis ("Vertical");
			jump = Input.GetAxis ("Jump");

			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				isWalking = !isWalking;
				animator.SetBool ("IsWalking", isWalking);
			}

			if (horizontal > 0 || horizontal < 0)
			{
				animator.SetFloat ("MoveX", Input.GetAxis("Horizontal"));
				StartMoving ();
			}

			if (vertical > 0 || vertical < 0)
			{
				animator.SetFloat ("MoveZ", Input.GetAxis("Vertical"));
				StartMoving ();
			}

			if (vertical == 0 && horizontal == 0)
			{
				StopMoving ();
			}

		}

		private void StartMoving()
		{
			animator.SetFloat ("Speed", 1f);
		}

		private void StopMoving()
		{
			animator.SetFloat ("Speed", 0f);
		}
		#endregion
	}
}