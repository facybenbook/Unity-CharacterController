using System.Collections;
using System.Collections.Generic;
using TPS.Configurations;
using UnityEngine;

namespace TPS.CharacterController
{
	[DisallowMultipleComponent]
	[RequireComponent (typeof (Animator))]
	[RequireComponent (typeof (CharacterMovement))]
	[RequireComponent (typeof (CharacterInventory))]
	public class CharacterIK : MonoBehaviour
	{
		#region References
		public CharacterStatus characterStatus;
		public Transform targetLook;
		public Transform leftHandTargetPosition;
		public Transform rightHandTargetPosition;

		private Animator characterAnimator;
		private CharacterMovement characterMovement;
		private CharacterInventory characterInventory;

		private Transform leftHand;
		private Transform rightHand;

		private Transform rightShoulder;
		private Transform aimPivot;
		#endregion

		#region Variables
		public Quaternion leftHandRotation;

		[Header ("Ik Default Weight")]
		public float weightDefault = 0.3f;
		public float bodyWeightDefault = 0.3f;
		public float headWeightDefault = 0.3f;

		[Header ("Ik Aiming Weight")]
		public float weightAiming = 1f;
		public float bodyWeightAiming = 0f;
		public float headWeightAiming = 1f;

		[Header ("Ik Hands Weight")]
		public float leftHandWeight = 1f;
		public float leftHandWeightSpeed = 0.5f;
		public float rightHandWeight = 1f;
		public float rightHandWeightSpeed = 0.5f;
		#endregion


		private void Awake ()
		{
			characterMovement = GetComponent<CharacterMovement> ();
			characterInventory = GetComponent<CharacterInventory> ();
			characterAnimator = GetComponent<Animator> ();
		}

		void Start ()
		{
			rightShoulder = characterAnimator.GetBoneTransform (HumanBodyBones.RightShoulder).transform;

			aimPivot = new GameObject ().transform;
			aimPivot.name = "AimPivot";
			aimPivot.transform.parent = transform;

			rightHand = new GameObject ().transform;
			rightHand.name = "RightHand";
			rightHand.parent = aimPivot;

			leftHand = new GameObject ().transform;
			leftHand.name = "LeftHand";
			leftHand.parent = aimPivot;

			rightHand.localPosition = characterInventory.secondWeapon.righHandPosition;
			Quaternion rotationRightHand = Quaternion.Euler (characterInventory.secondWeapon.righHandRotation);
			rightHand.localRotation = rotationRightHand;
		}

		void Update ()
		{
			leftHandRotation = leftHandTargetPosition.rotation;
			leftHand.position = leftHandTargetPosition.position;

			rightHand.localPosition = characterInventory.secondWeapon.righHandPosition;
			Quaternion rotationRightHand = Quaternion.Euler (characterInventory.secondWeapon.righHandRotation);
			rightHand.localRotation = rotationRightHand;

			if (characterStatus.isAiming)
			{
				rightHandWeight += Time.deltaTime * rightHandWeightSpeed;
				// leftHandWeight += Time.deltaTime * leftHandWeightSpeed;
			}
			else
			{
				rightHandWeight -= Time.deltaTime * rightHandWeightSpeed;
				// leftHandWeight -= Time.deltaTime * leftHandWeightSpeed;
			}

			rightHandWeight = Mathf.Clamp01 (rightHandWeight);
			// leftHandWeight = Mathf.Clamp01(leftHandWeight);

		}

		private void OnAnimatorIK (int layerIndex)
		{
			aimPivot.position = rightShoulder.position;
			aimPivot.LookAt (targetLook);

			if (!characterStatus.isAiming)
			{
				characterAnimator.SetLookAtWeight (weightDefault, bodyWeightDefault, headWeightDefault);
			}
			else
			{
				characterAnimator.SetLookAtWeight (weightAiming, bodyWeightAiming, headWeightAiming);
			}

			characterAnimator.SetLookAtPosition (targetLook.position);

			characterAnimator.SetIKPositionWeight (AvatarIKGoal.LeftHand, leftHandWeight);
			characterAnimator.SetIKRotationWeight (AvatarIKGoal.LeftHand, leftHandWeight);
			characterAnimator.SetIKPosition (AvatarIKGoal.LeftHand, leftHand.position);
			characterAnimator.SetIKRotation (AvatarIKGoal.LeftHand, leftHandRotation);

			characterAnimator.SetIKPositionWeight (AvatarIKGoal.RightHand, rightHandWeight);
			characterAnimator.SetIKRotationWeight (AvatarIKGoal.RightHand, rightHandWeight);
			characterAnimator.SetIKPosition (AvatarIKGoal.RightHand, rightHand.position);
			characterAnimator.SetIKRotation (AvatarIKGoal.RightHand, rightHand.rotation);
		}
	}
}

