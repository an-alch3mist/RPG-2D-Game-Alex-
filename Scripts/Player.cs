using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class Player : MonoBehaviour
	{
		StateMachine SM;
		#region new input system
		public PlayerInputActions inputAction; 
		#endregion
		public Vector2 inpVel { get; private set; }

		Player_IdleState idleState;
		[Header("movment config")]
		[SerializeField] float xSpeed = 8;
		[SerializeField] float jumpForce = 5;

		private void Awake()
		{
			Debug.Log("Awake(): " + this);

			#region new input system
			inputAction = new PlayerInputActions(); 
			#endregion

			// 0. create state machine
			this.SM = new StateMachine("player", new PlayerInfo()
			{
				obj = this.gameObject,
				animator = this.gameObject.NameStartsWith("visual").GC<Animator>(),
				rb = this.gameObject.GC<Rigidbody2D>(),
				player = this,
			});

			// 1. create entity state(stateType, ref stateMachine) (store is done auto while creation)
			idleState = new Player_IdleState(StateType.player_idle, SM);
			Player_MoveState moveState = new Player_MoveState(StateType.player_move, SM);
			Player_JumpState jumpState = new Player_JumpState(StateType.player_jump, SM);
			Player_FallState fallState = new Player_FallState(StateType.player_fall, SM);
			LOG.SaveLog(SM.MAP_STATE.ToTable(name: "MAP_STATE<>"));
		}

		private void OnEnable()
		{
			#region new input system
			// enable gameObject
			this.inputAction.Enable();

			/*
			inputAction.player.movement.started;	// instant down
			inputAction.player.movement.performed;	// held down
			inputAction.player.movement.canceled;	// instant up
			*/
			inputAction.player.movement.performed += (ctx) => this.inpVel = ctx.ReadValue<Vector2>();
			inputAction.player.movement.canceled  += (ctx) => this.inpVel = new Vector2(0, 0);
			#endregion
		}

		private void OnDisable()
		{
			#region new input system
			// disable or destroy gameObject
			// unsubscribe from all audience
			this.inputAction.Disable(); 
			#endregion
		}

		private void Start()
		{
			SM.GoTo(this.idleState);
		}

		private void Update()
		{
			SM.UpdateCurrentState();
		}

		#region Method
		public void HandleMovement()
		{
			var rb = SM.info.rb;
			SM.info.rb.velocity = new Vector2(this.xSpeed * this.inpVel.x, rb.velocity.y);
		}

		public void HandleFlip()
		{
			var transform = SM.info.obj.gameObject.transform; var rb = SM.info.rb;
			int currFacingDir = C.sign(rb.velocity.x);
			if (currFacingDir == 0)
				return;
			transform.localScale = new Vector3(currFacingDir, 1, 1);
		}

		public void HandleYVel()
		{
			var animator = SM.info.animator; var rb = SM.info.rb;
			animator.SetFloat("player_yvel", rb.velocity.y);
		}

		public void HandleJump()
		{
			var rb = SM.info.rb;
			rb.AddForce(Vector2.up * this.jumpForce);
		}
		#endregion
	}
}
