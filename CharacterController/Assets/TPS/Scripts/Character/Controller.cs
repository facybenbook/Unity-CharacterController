using UnityEngine;

namespace TPS.CharacterController
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CharacterMovement))]
	[RequireComponent(typeof(CharacterAnimation))]
	[RequireComponent(typeof(CharacterInput))]
	public class Controller : MonoBehaviour
	{
		private CharacterMovement characterMovement;
		private CharacterAnimation characterAnimation;
		private CharacterInput characterInput;

		private void Awake ()
		{
			characterMovement = GetComponent<CharacterMovement>();
			characterAnimation = GetComponent<CharacterAnimation> ();
			characterInput = GetComponent<CharacterInput> ();
		}

		private void Update ()
		{
			characterMovement.RunUpdate ();
			characterInput.InputUpdate ();
		}

		public void FixedUpdate ()
		{
			characterMovement.MoveUpdate ();
			characterAnimation.AnimationUpdate ();
		}
	}
}