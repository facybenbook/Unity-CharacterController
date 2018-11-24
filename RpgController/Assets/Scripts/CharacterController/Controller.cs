using UnityEngine;

namespace CharacterController
{
	[RequireComponent(typeof(CharacterMovement))]
	public class Controller : MonoBehaviour
	{
		private CharacterMovement characterMovement;

		private void Awake ()
		{
			characterMovement = GetComponent<CharacterMovement>();
		}

		public void FixedUpdate ()
		{
			characterMovement.MoveUpdate ();
		}
	}
}