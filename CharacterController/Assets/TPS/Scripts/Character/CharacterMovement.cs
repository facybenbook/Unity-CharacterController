using UnityEngine;

namespace TPS.CharacterController
{
	[RequireComponent(typeof(Animator))]
	public class CharacterMovement : MonoBehaviour
	{
		#region References
		private Animator animator;
		#endregion References

		#region Variables
		[HideInInspector]
		public float vertical;
		[HideInInspector]
		public float horizontal;
		[HideInInspector]
		public float moveSpeed;
		[HideInInspector]
		public float dampTime = 0.15f;
		#endregion Variables

		#region Animator Parameters
		[HideInInspector]
		public const string VERTICAL = "Vertical";
		[HideInInspector]
		public const string HORIZONTAL = "Horizontal";
		#endregion Animator Parameters

		public void Awake ()
		{
			animator = GetComponent<Animator> ();
		}

		public void MoveUpdate ()
		{
			vertical = Input.GetAxis (VERTICAL);
			horizontal = Input.GetAxis (HORIZONTAL);
			moveSpeed = Mathf.Clamp01 (Mathf.Abs (vertical) + Mathf.Abs (horizontal));

			animator.SetFloat (VERTICAL, vertical, dampTime, Time.deltaTime);
		}
	}
}