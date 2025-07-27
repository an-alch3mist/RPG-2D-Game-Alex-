using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPACE_RPG2D
{
	public class AnimationEVENT : MonoBehaviour
	{
		public void Player_BasicAttackOver()
		{
			Debug.Log("AnimationEvent: BasicAttackOver()");
			var player = this.transform.parent.gameObject.GetComponent<Player>();
			player.basicAttackOver = true; // reset is done when entity state is entered each time

			// to use event subscribe approach, EntityState got to have Awake() method which runs at very first enter
		}
	}

}