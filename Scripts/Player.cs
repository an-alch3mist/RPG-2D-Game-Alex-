using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class Player : MonoBehaviour, IComponentInfo
	{
		#region IComponentInfo
		public GameObject Obj() => this.gameObject;
		public Animator animator() => this.gameObject.Query("visual").GC<Animator>();
		public Rigidbody2D rb() => this.gameObject.GC<Rigidbody2D>();
		#endregion

		private void Awake()
		{
			Debug.Log("Awake(): " + this);

			// 0. create state machine
			StateMachine stateMachine = new StateMachine(); 

			// 1. create entity state, ref the stateType, stateMachine, info
			Player_IdleState idleState = new Player_IdleState(StateType.player_idle, stateMachine, this);

			// 2. add created entity state to MAP STATE
			stateMachine.MAP_STATE[StateType.player_idle] = idleState;

			stateMachine.GoTo(idleState);
			Debug.Log('-'.repeat(100));
			stateMachine.UpdateCurrentState();
		}
	}
}
