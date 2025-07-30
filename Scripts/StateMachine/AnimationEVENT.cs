using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPACE_RPG2D
{
	public class AnimationEVENT : MonoBehaviour
	{
		#region event subscribe approach
		public static event EventHandler _subscribeChannel_WhenBasicAttackAnimationComplete; 
		#endregion

		public void Player_BasicAttackAnimationComplete()
		{
			Debug.Log("AnimationEvent: BasicAttackOver()");
			// to use event subscribe approach, EntityState got to have Awake() method which runs at very first enter
			// notify sunscribers >>
			_subscribeChannel_WhenBasicAttackAnimationComplete? // if subscriber count is not zero, otherwise error
				.Invoke(this, EventArgs.Empty);
			// << notify subscribers
		}
	}

}