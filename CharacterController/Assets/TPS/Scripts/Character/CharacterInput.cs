using UnityEngine;
using TPS.CameraController;
using TPS.Configurations;

namespace TPS.CharacterController
{
	public class CharacterInput : MonoBehaviour
	{
		#region References
		public CharacterStatus characterStatus;
		#endregion

		#region Input Variables
		public bool debugAiming;
		public bool isAiming;
		#endregion

		public void InputUpdate ()
		{
			if (!debugAiming)
			{
				characterStatus.isAiming = Input.GetMouseButton (Statics.Mouse_rightClick);
			}
			else
			{
				characterStatus.isAiming = isAiming;
			}
		}
	}
}