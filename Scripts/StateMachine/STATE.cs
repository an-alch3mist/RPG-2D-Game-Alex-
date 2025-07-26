using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SPACE_UTIL;

namespace SPACE_RPG2D
{
	// used as >>
	public class Player_State : EntityState
	{
		public Player_State(StateMachine stateMachine) : base(StateType.none, stateMachine)
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
	// << used as

	#region Ground
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

			var player = SM.info.player; var rb = SM.info.rb;
			// handle x movement
			player.HandleXMovement();
			player.HandleXFlip();

			// handle y detection + anim
			player.HandleYCollisionDetection();
			player.HandleYAnim();

			#region GoTo
			// fall
			// non-zero negative check
			if (rb.velocity.y < -C.e) // could also use ground detection == false (wont work with wider collider with narrow width ray cast)
				SM.GoTo(StateType.player_fall);
			// jump
			else if (player.groundDetected == true)
			{
				if (player.inputAction.player.jump.WasPerformedThisFrame())
				{
					SM.GoTo(StateType.player_jump);
					player.groundDetected = false;
				}
			}
			#endregion
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
	public class Player_IdleState : Player_GroundState
	{
		public Player_IdleState(StateMachine stateMachine) : base(StateType.player_idle, stateMachine)
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
				SM.GoTo(StateType.player_move);
			#endregion
		}
		public override void Exit()
		{
			base.Exit();
		}

	}
	public class Player_MoveState : Player_GroundState
	{
		public Player_MoveState(StateMachine stateMachine) : base(StateType.player_move, stateMachine)
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
				SM.GoTo(StateType.player_idle);
			#endregion
		}
		public override void Exit()
		{
			base.Exit();
		}
	}
	#endregion

	#region Air
	public class Player_AirState : EntityState
	{
		public Player_AirState(StateType stateType, StateMachine stateMachine, StateType blendStateType) : base(stateType, stateMachine, blendStateType)
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

			// handle x movement
			player.HandleXAirMovement();
			player.HandleXFlip();

			// handle y detection + anim
			player.HandleYCollisionDetection();
			player.HandleYAnim();
		}

		public override void Exit()
		{
			base.Exit();
		}

	}
	public class Player_JumpState : Player_AirState
	{
		public Player_JumpState(StateMachine stateMachine) : base(StateType.player_jump, stateMachine, StateType.player_air)
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
			#region GoTo
			if (rb.velocity.y < 0f)
				SM.GoTo(StateType.player_fall); 
			#endregion
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
	public class Player_FallState : Player_AirState
	{
		public Player_FallState(StateMachine stateMachine) : base(StateType.player_fall, stateMachine, StateType.player_air)
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
			#region GoTo
			if (player.groundDetected == true)
				SM.GoTo(StateType.player_idle); 
			#endregion
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
	#endregion
}