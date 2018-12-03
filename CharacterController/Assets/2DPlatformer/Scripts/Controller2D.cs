using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Controller;

namespace Platformer.Player.Controller
{
	public class Controller2D : RaycastController
	{
		#region References
		public CollisionInfo collisionInfo;
		#endregion

		#region Variables
		public float maxClimbAngle = 80f;
		public float maxDescendAngle = 80f;
		#endregion

		public override void Start() 
		{
			base.Start();	
		}

		#region Controller2D Methods
        public void Move(Vector3 velocity)
        {
			UpdateRaycastOrigins ();
			collisionInfo.Reset();

			if (velocity.y < 0)
			{
				DescendSlope(ref velocity);
			}
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
						if (collisionInfo.descendingSlope)
						{
							collisionInfo.descendingSlope = false;
							velocity = collisionInfo.velocityOld;
						}
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

				if (collisionInfo.climbingSlope)
				{
					float directionX = Mathf.Sign(velocity.x);
					rayLength = Mathf.Abs(velocity.x) + skinWidth;
					rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;

					hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX,rayLength, collisionMask);

					if (hit)
					{
						float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
						if (slopeAngle != collisionInfo.slopeAngle)
						{
							velocity.x = (hit.distance - skinWidth) * directionX;
							collisionInfo.slopeAngle = slopeAngle;
						}
					}

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

		 private void DescendSlope(ref Vector3 velocity)
        {
            float directionX = Mathf.Sign(velocity.x);
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

			if (hit)
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
				{
					if (Mathf.Sign(hit.normal.x) == directionX)
					{
						if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
						{
							float moveDistance = Mathf.Abs(velocity.x);
							float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
							
							velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
							velocity.y -= descendVelocityY;

							collisionInfo.slopeAngle = slopeAngle;
							collisionInfo.descendingSlope = true;
							collisionInfo.below = true;
						}
					}
				} 
			}
        }
		#endregion

		public struct CollisionInfo
		{
			public Vector3 velocityOld;

			public bool above, below;
			public bool left, right;
			public bool climbingSlope;
            public bool descendingSlope;

			public float slopeAngle, slopeAngleOld;

            public void Reset()
			{
				velocityOld = Vector3.zero;

				above = below = false;
				left = right = false;
				climbingSlope = false;
				descendingSlope = false;

				slopeAngleOld = slopeAngle;
				slopeAngle = 0;
			}
		}
	}
}