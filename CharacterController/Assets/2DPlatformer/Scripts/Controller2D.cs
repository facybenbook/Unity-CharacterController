using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Player.Controller
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class Controller2D : MonoBehaviour 
	{
		#region References
		BoxCollider2D _collider;
		RaycastOrigins raycastOrigins;
		#endregion

		#region Variables
		private const float skinWidth = 0.015f;

		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		private float horizontalRaySpacing;
		private float verticalRaySpacing; 
		#endregion

		void Start () 
		{
			_collider = GetComponent<BoxCollider2D> ();
		}

		private void Update() 
		{
			UpdateRaycastOrigins ();
			CalculateRaySpacing ();

			for (int i = 0; i < verticalRayCount; i++)
			{
				// To top
				Debug.DrawRay(raycastOrigins.topLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * 2, Color.red);
				// To bottom
				Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -2, Color.red);
				// Ro right
				Debug.DrawRay(raycastOrigins.topRight + -Vector2.up * horizontalRaySpacing * i, Vector2.right * 2, Color.red);
				// To left
				Debug.DrawRay(raycastOrigins.topLeft + -Vector2.up * horizontalRaySpacing * i, Vector2.right * -2, Color.red);
			}	
		}
		
		void UpdateRaycastOrigins ()
		{
			Bounds bounds = _collider.bounds;
			bounds.Expand(skinWidth * -2);

			// Get each side of the player's bounds and store them in the RaycastOrigins container
			raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
			raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
			raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
			raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
		}

		void CalculateRaySpacing ()
		{
			Bounds bounds = _collider.bounds;
			bounds.Expand(skinWidth * -2);

			// Fix value of the ray count
			horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
			verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

			// Get Spacing by using size of the player's bounds 
			horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
			verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

		}
		
		struct RaycastOrigins
		{
			public Vector2 topLeft, topRight;
			public Vector2 bottomLeft, bottomRight;
		}
	}
}