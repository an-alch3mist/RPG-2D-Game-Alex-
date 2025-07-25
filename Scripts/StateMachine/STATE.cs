using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class Player_GroundState : EntityState
	{
		public Player_GroundState(StateType stateType, StateMachine stateMachine) : base(stateType, stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}

		public override void Update()
		{
			base.Update();
			// GoTo if required

			var player = SM.info.player;

			// handle Jump
			if (player.inputAction.player.jump.WasPerformedThisFrame())
			{
				Debug.Log("Transition to Player_JumpState");
				SM.GoTo(SM.MAP_STATE[StateType.player_jump]);
			}
		}

		public override void Exit()
		{
			base.Exit();
		}
	}

	public class Player_IdleState : Player_GroundState
	{
		public Player_IdleState(StateType stateType, StateMachine stateMachine) : base(stateType, stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}
		public override void Update()
		{
			base.Update();
			// GoTo When required
			#region GoTo
			var player = SM.info.player;
			if (player.inpVel.x != 0)
				base.SM.GoTo(base.SM.MAP_STATE[StateType.player_move]); 
			#endregion
		}
		public override void Exit()
		{
			base.Exit();
		}

	}

	public class Player_MoveState : Player_GroundState
	{
		public Player_MoveState(StateType stateType, StateMachine stateMachine) : base(stateType, stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}
		public override void Update()
		{
			base.Update();
			// GoTo When required
			var player = SM.info.player;
			#region GoTo
			if (player.inpVel.x == 0)
				base.SM.GoTo(base.SM.MAP_STATE[StateType.player_idle]);
			#endregion

			player.HandleMovement();
			player.HandleFlip();
		}
		public override void Exit()
		{
			base.Exit();
		}
	}

	public class Player_State : EntityState
	{
		public Player_State(StateType stateType, StateMachine stateMachine) : base(stateType, stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}
		public override void Update()
		{
			base.Update();
			// GoTo When required
		}
		public override void Exit()
		{
			base.Exit();
		}
	}


	public class Player_JumpState : EntityState
	{
		public Player_JumpState(StateType stateType, StateMachine stateMachine) : base(stateType, stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
			var player = SM.info.player;
			player.HandleJump();
		}

		public override void Update()
		{
			base.Update();
			var player = SM.info.player; var rb = SM.info.rb;
			// GoTo if required
			if (rb.velocity.y < 0f)
				SM.GoTo(SM.MAP_STATE[StateType.player_fall]);

			player.HandleYVel();
		}

		public override void Exit()
		{
			base.Exit();
		}
	}

	public class Player_FallState : EntityState
	{
		public Player_FallState(StateType stateType, StateMachine stateMachine) : base(stateType, stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
		}

		public override void Update()
		{
			base.Update();
			var player = SM.info.player;
			// GoTo if required
			player.HandleYVel();
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
}