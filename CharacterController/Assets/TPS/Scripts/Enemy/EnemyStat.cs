using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS.Enemy
{
	public class EnemyStat : MonoBehaviour 
	{
		public Rigidbody[] rigidBody;
		private Animator animator;
		public int health;

		void Awake () 
		{
			animator = GetComponent<Animator>();
		}

		public void SetDamage (int damage)
		{
			if (damage <= health)
			{
				health -= damage;
			}
			else
			{
				health = 0;
				Dead ();
			}
		}

		public void Dead ()
		{
			foreach (Rigidbody rb in rigidBody)
			{
				rb.isKinematic = false;
			}
			animator.enabled = false;
		}
	}
}
