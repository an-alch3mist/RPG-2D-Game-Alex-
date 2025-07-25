using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class Player_IdleState : EntityState
	{
		public Player_IdleState(StateType stateType, StateMachine stateMachine, IComponentInfo info) : base(stateType, stateMachine, info)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}
		public override void Update()
		{
			base.Update();
			// change to other state when required, using state machine
		}
		public override void Exit()
		{
			base.Exit();
		}

	}

	public class Player_MoveState : EntityState
	{
		public Player_MoveState(StateType stateType, StateMachine stateMachine, IComponentInfo info) : base(stateType, stateMachine, info)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}
		public override void Update()
		{
			base.Update();
		}
		public override void Exit()
		{
			base.Exit();
		}
	}

	public class Player_State : EntityState
	{
		public Player_State(StateType stateType, StateMachine stateMachine, IComponentInfo info) : base(stateType, stateMachine, info)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}
		public override void Update()
		{
			base.Update();
		}
		public override void Exit()
		{
			base.Exit();
		}
	}
}