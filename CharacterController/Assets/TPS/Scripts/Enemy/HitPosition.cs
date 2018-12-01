using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPS.Enemy
{
	public class HitPosition : MonoBehaviour 
	{
		public EnemyStat enemyStat;
		public int multiplicationDamage;
		
		public void Damage (int damage) 
		{
			enemyStat.SetDamage(damage * multiplicationDamage);
		}
	}
}

