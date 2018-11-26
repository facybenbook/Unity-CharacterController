using UnityEngine;
using TPS.CameraController;
using TPS.Configurations;

namespace TPS.CharacterController
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CharacterMovement))]
	public class CharacterAnimation : MonoBehaviour
	{
		Animator animator;
		CharacterMovement characterMovement;
		public CharacterStatus characterStatus;

		private float dampTime = 0.15f;

		private void Awake ()
		{
			animator = GetComponent<Animator> ();
			characterMovement = GetComponent<CharacterMovement> ();
		}

		public void AnimationUpdate ()
		{
			animator.SetBool (Statics.Sprint, characterStatus.isSprint);
			animator.SetBool (Statics.Aiming, characterStatus.isAiming);
			animator.SetBool (Statics.Weapon, characterStatus.isWeapon);

			if (!characterStatus.isAiming)
			{
				AnimationDefault ();
				animator.SetFloat (Statics.Speed, characterMovement.movingSpeedFoAnimation);
			}

			if (characterStatus.isAiming)
			{
				AnimationAiming ();
			}
		}

		private void AnimationDefault()
		{
			animator.SetFloat (Statics.Vertical, characterMovement.movingSpeedFoAnimation, dampTime, Time.deltaTime);
		}

		private void AnimationAiming()
		{
			animator.SetFloat (Statics.Vertical, characterMovement.verticalInput, dampTime, Time.deltaTime);
			animator.SetFloat (Statics.Horizontal, characterMovement.horizontalInput, dampTime, Time.deltaTime);
		}
	}
}