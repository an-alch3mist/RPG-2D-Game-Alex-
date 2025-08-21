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
			player.HandleXDirFlip();

			// handle xy detection + y anim
			player.HandleXYCollisionDetection();
			player.HandleYAnim();

			#region GoTo
			// fall
			// non-zero negative check
			if (rb.velocity.y < -C.e) // could also use ground detection == false (wont work with wider collider with narrow width ray cast)
			{
				if (player.groundDetected == false) // if within ground detection arrow range stay in idle
					SM.GoTo(StateType.player_fall);
			}
			// jump
			else if (player.groundDetected == true || rb.velocity.y.zero())
			{
				if (player.inputAction.player.jump.WasPerformedThisFrame())
				{
					SM.GoTo(StateType.player_jump);
					player.groundDetected = false;
				}
			}

			if(player.inputAction.player.attack.WasPressedThisFrame()) // instant down
				SM.GoTo(StateType.player_basicattack);
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
				if (player.wallDetected == false) // if no wall ahead
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
			player.HandleXDirFlip();

			// handle xy detection + y anim
			player.HandleXYCollisionDetection();
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
	// TODO fall as parabolic after WallJump -> FallState, independent on InpVel
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
			var player = SM.info.player; var rb = SM.info.rb;
			// GoTo if required
			#region GoTo
			if (player.groundDetected == true || rb.velocity.y.zero())
				SM.GoTo(StateType.player_idle);

			if (player.wallDetected == true)
				SM.GoTo(StateType.player_wallslide);
			#endregion
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
	#endregion

	#region Wall
	public class Player_WallSlideState : EntityState
	{
		public Player_WallSlideState(StateMachine stateMachine) : base(StateType.player_wallslide, stateMachine)
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
			player.HandleXDirFlip();

			// handle xy detection + y anim
			player.HandleXYCollisionDetection();
			// handle wall slide
			player.HandleWallSlide();

			// GoTo when required
			#region GoTo
			if (player.groundDetected == true)
				SM.GoTo(StateType.player_idle);

			if (player.wallDetected == false)
				SM.GoTo(StateType.player_fall);

			if (player.inputAction.player.jump.WasPerformedThisFrame()) // instant down this frame?
				SM.GoTo(StateType.player_walljump);
			#endregion
		}
		public override void Exit()
		{
			base.Exit();
		}
	}
	public class Player_WallJumpState : EntityState
	{
		public Player_WallJumpState(StateMachine stateMachine) : base(StateType.player_walljump, stateMachine, StateType.player_air)
		{
		}

		public override void Enter()
		{
			base.Enter();

			var player = SM.info.player; var rb = SM.info.rb;
			player.HandleWallJump(); // perform jump when state is entered
			Debug.Log(rb.velocity);
			player.HandleXDirFlip();
		}
		public override void Update()
		{
			base.Update();
			var player = SM.info.player; var rb = SM.info.rb;

			// handle x movement
			//player.HandleXAirMovement(); // donot listen to inpVel vec2 during wall jump
			player.HandleXDirFlip();

			// handle y anim parameter
			player.HandleYAnim();

			// handle xy collision detection
			player.HandleXYCollisionDetection();

			// GoTo when required
			#region GoTo
			// transition to player_fall : similar to Player_JumpState
			//  transition to player_wallslide : similar to Player_Fall
			if (rb.velocity.y < 0f)
				SM.GoTo(StateType.player_fall);
			if (player.wallDetected == true)
				SM.GoTo(StateType.player_wallslide);
			#endregion
		}
		public override void Exit()
		{
			base.Exit();
		}
	}
	#endregion

	#region Dash

	#endregion

	#region Attack
	public class Player_BasicAttackState : EntityState
	{
		public Player_BasicAttackState(StateMachine stateMachine) : base(StateType.player_basicattack, stateMachine)
		{
			// at the very first time creation of state => subscribe
			#region event subscriber approach
			AnimationEVENT._subscribeChannel_WhenBasicAttackAnimationComplete += (o, e) =>
			{
				var rb = SM.info.rb;
				// rb.velocity = new Vector2(0f, rb.velocity.y); // stop movementarily and strike
				
				// GoTo when required
				#region GoTo
				SM.GoTo(StateType.player_idle);
				#endregion
			};
			#endregion
		}

		int comboattack_index = 0;
		public override void Enter()
		{
			// set the comboindex, set the bool, all occured in same frame, order doesnt matter
			var animator = SM.info.animator;

			animator.SetInteger("player_comboattack_index", this.comboattack_index);
			this.comboattack_index = (this.comboattack_index + 1) % 2;
			base.Enter(); // animation bool is set
		}
		public override void Update()
		{
			base.Update();
			var player = SM.info.player;
			player.HandleXAttackMovement();

			// GoTo revisit
			#region GoTo
			if (player.inputAction.player.attack.WasPerformedThisFrame())
				SM.GoToRevisit();
			#endregion
		}
		public override void Exit()
		{
			base.Exit();
		}
	}
	#endregion
}