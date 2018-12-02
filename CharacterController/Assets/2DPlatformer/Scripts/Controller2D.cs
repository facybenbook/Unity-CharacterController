using System;
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
		public CollisionInfo collisionInfo;
		public LayerMask collisionMask;
		#endregion

		#region Variables
		private const float skinWidth = 0.015f;

		public float maxClimbAngle = 80f;

		public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

		private float horizontalRaySpacing;
		private float verticalRaySpacing; 
		#endregion

		void Start () 
		{
			_collider = GetComponent<BoxCollider2D> ();
			CalculateRaySpacing ();
		}
		
		#region Controller2D Methods
        public void Move(Vector3 velocity)
        {
			UpdateRaycastOrigins ();
			collisionInfo.Reset();

			if (velocity.y != 0)
			{
				VerticalCollisions(ref velocity);
			}
			if (velocity.x != 0)
			{
				HorizontalCollisions(ref velocity);
			}

            transform.Translate(velocity);
        }

		void HorizontalCollisions(ref Vector3 velocity)
		{
			float directionX = Mathf.Sign(velocity.x);
			float rayLength = Mathf.Abs(velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

				if (hit)
				{
					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

					if (i == 0 && slopeAngle <= maxClimbAngle)
					{
						float distanceToSlopeStart = 0f;

						if (slopeAngle != collisionInfo.slopeAngleOld)
						{
							distanceToSlopeStart = hit.distance - skinWidth;
							velocity.x -= distanceToSlopeStart * directionX;
						}

						ClimbSlope(ref velocity, slopeAngle);
						velocity.x += distanceToSlopeStart * directionX;

						Debug.Log("Slope angle is " + slopeAngle);
					}

					if (!collisionInfo.climbingSlope || slopeAngle > maxClimbAngle)
					{
						velocity.x = Mathf.Min(Mathf.Abs(velocity.x), (hit.distance - skinWidth)) * directionX;
						rayLength = Mathf.Min(Mathf.Abs(velocity.x) + skinWidth, hit.distance);

						if (collisionInfo.climbingSlope)
						{
							velocity.y = Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
						}

						collisionInfo.left = (directionX == -1);
						collisionInfo.right = (directionX == 1);
					}
				}
			}
		}

        void VerticalCollisions(ref Vector3 velocity)
		{
			float directionY = Mathf.Sign(velocity.y);
			float rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * ( verticalRaySpacing * i + velocity.x); 
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

				if (hit)
				{
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;

					if (collisionInfo.climbingSlope)
					{
						velocity.x = velocity.y / Mathf.Tan(collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
					}

					collisionInfo.below = (directionY == -1);
					collisionInfo.above = (directionY == 1);
				}
			}
		}

		private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(velocity.x);
			float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
			
			if ( velocity.y <= climbVelocityY)
			{
				velocity.y = climbVelocityY;
				velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);

				collisionInfo.below = true;
				collisionInfo.climbingSlope = true;
				collisionInfo.slopeAngle = slopeAngle;
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
		#endregion
		
		struct RaycastOrigins
		{
			public Vector2 topLeft, topRight;
			public Vector2 bottomLeft, bottomRight;
		}

		public struct CollisionInfo
		{
			public bool above, below;
			public bool left, right;
			public bool climbingSlope;

			public float slopeAngle, slopeAngleOld;

			public void Reset()
			{
				above = below = false;
				left = right = false;
				climbingSlope = false;

				slopeAngleOld = slopeAngle;
				slopeAngle = 0;
			}
		}
	}
}