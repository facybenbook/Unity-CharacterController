using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Controller
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class RaycastController : MonoBehaviour 
	{
		#region References
		BoxCollider2D _collider;
		[HideInInspector]
		public RaycastOrigins raycastOrigins; 
		public LayerMask collisionMask;
		#endregion

		#region Variables
		[HideInInspector]
		public const float skinWidth = 0.015f;

		public int horizontalRayCount = 4;
		public int verticalRayCount = 4;

		[HideInInspector]
		public float horizontalRaySpacing;
		[HideInInspector]
		public float verticalRaySpacing; 
		#endregion
		
		public virtual void Start () 
		{
			_collider = GetComponent<BoxCollider2D> ();
			CalculateRaySpacing ();
		}

		public 	void UpdateRaycastOrigins ()
		{
			Bounds bounds = _collider.bounds;
			bounds.Expand(skinWidth * -2);

			// Get each side of the player's bounds and store them in the RaycastOrigins container
			raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
			raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
			raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
			raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
		}

		public void CalculateRaySpacing ()
		{
			Bounds bounds = _collider.bounds;
			bounds.Expand(skinWidth * -2);

			// Fix value of the ray's count
			horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
			verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

			// Get Spacing by using size of the player's bounds 
			horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
			verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

			// Debug.Log(bounds.size.y);
			// Debug.Log(bounds.size.x);
			// Debug.Log(horizontalRaySpacing);
			// Debug.Log(verticalRaySpacing);
		}

		public struct RaycastOrigins
		{
			public Vector2 topLeft, topRight;
			public Vector2 bottomLeft, bottomRight;
		}
	}
}
