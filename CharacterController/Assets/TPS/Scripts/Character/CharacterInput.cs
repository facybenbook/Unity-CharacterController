using UnityEngine;
using TPS.CameraController;
using TPS.Configurations;
using TPS.WeaponController;

namespace TPS.CharacterController
{
	public class CharacterInput : MonoBehaviour
	{
		#region References
		public CharacterStatus characterStatus;
		public Weapon weapon;
		public Transform targetLook;
		#endregion

		#region Input Variables
		public bool debugAiming;
		public bool isAiming;

		public float shoulderHeightPosition = 1.4f;

		public bool canAim;
		private float distanceToObstacle;
		#endregion

		private void Awake() 
		{
			targetLook = weapon.targetLook;
		}

		public void InputUpdate ()
		{
			RayCastAiming ();

			if (Input.GetMouseButton(Statics.Mouse_rightClick) && canAim)
			{
				characterStatus.isAiming = true;
				characterStatus.isMoving = true;
			}
			if (Input.GetMouseButton(Statics.Mouse_rightClick) && !canAim)
			{
				characterStatus.isAiming = false;
				characterStatus.isMoving = true;
			}
			if (!Input.GetMouseButton(Statics.Mouse_rightClick))
			{
				characterStatus.isAiming = false;
				characterStatus.isMoving = false;
			}

			if (!debugAiming)
			{
				characterStatus.isAiming = Input.GetMouseButton (Statics.Mouse_rightClick);
			}
			else
			{
				characterStatus.isAiming = true;
				characterStatus.isMoving = true;
			}

			if (Input.GetMouseButtonDown(Statics.Mouse_leftClick) 
				&& Input.GetMouseButton(Statics.Mouse_rightClick) 
				&& canAim)
			{
				weapon.Shoot();
			}
		}

		public void RayCastAiming()
		{
			Debug.DrawLine(transform.position + transform.up * shoulderHeightPosition, targetLook.position, Color.yellow);

			distanceToObstacle = Vector3.Distance(transform.position + transform.up * shoulderHeightPosition, targetLook.position);
			canAim = distanceToObstacle > 1.5f;

		}
	}
}