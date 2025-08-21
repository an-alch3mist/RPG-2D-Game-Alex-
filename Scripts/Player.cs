using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public bool groundDetected, wallDetected;

		[Header("movment config")]
		[SerializeField] float xSpeed = 8;
		[SerializeField] float xAirSpeed = 6;
		[SerializeField] [Range(0.05f, 1f)] float ySpeedWallMultiplier = 0.3f;
		[SerializeField] float jumpSpeed = 15;
		[SerializeField] float wallJumpSpeed = 10;
		[SerializeField] float basicAttackSpeed = 1f;

		[Header("collision config")]
		[SerializeField] float groundCheckDist = 1.5f;
		[SerializeField] float wallCheckDist = 0.5f;
		[SerializeField] [Range(0f, 1f)] float wallCheckRaysApartDist = 0.5f;
		[SerializeField] LayerMask goundLayer;

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
			new Player_IdleState(SM);
			new Player_MoveState(SM);
			new Player_JumpState(SM);
			new Player_FallState(SM);
			new Player_WallSlideState(SM);
			new Player_WallJumpState(SM);
			new Player_BasicAttackState(SM);
			SM.GoTo(StateType.player_idle); // idle or fall

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
			inputAction.player.movement.canceled += (ctx) => this.inpVel = new Vector2(0, 0);
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


		private void Update()
		{
			this.DrawCollisionDetection(this.gameObject.GC<Collider2D>().bounds.center);
			SM.UpdateCurrentState();
		}

		#region Method
		public void HandleXMovement()
		{
			var rb = SM.info.rb;
			SM.info.rb.velocity = new Vector2(this.xSpeed * this.inpVel.x, rb.velocity.y);
		}
		public void HandleXAirMovement()
		{
			var rb = SM.info.rb;
			SM.info.rb.velocity = new Vector2(this.xAirSpeed * this.inpVel.x, rb.velocity.y);
		}

		public void HandleXDirFlip()
		{
			var transform = SM.info.obj.gameObject.transform; var rb = SM.info.rb;
			int currFacingDir = C.sign(rb.velocity.x);
			if (currFacingDir == 0)
				return;
			transform.localScale = new Vector3(currFacingDir, 1, 1);
		}
		public int getXFacingDir
		{
			get { return C.sign(transform.localScale.x); }
		}

		public void HandleYAnim()
		{
			var animator = SM.info.animator; var rb = SM.info.rb;
			animator.SetFloat(StateType.player_yvel_float.ToString(), rb.velocity.y);
		}
		public void HandleXYCollisionDetection()
		{
			Vector2 center = this.gameObject.GetComponent<Collider2D>().bounds.center;
			this.groundDetected = Physics2D.Raycast(center, -Vector2.up, this.groundCheckDist, this.goundLayer);
			this.wallDetected =
				Physics2D.Raycast(center + Vector2.up * this.wallCheckRaysApartDist, Vector2.right * this.getXFacingDir, this.wallCheckDist, this.goundLayer) &&
				Physics2D.Raycast(center - Vector2.up * this.wallCheckRaysApartDist, Vector2.right * this.getXFacingDir, this.wallCheckDist, this.goundLayer);
		}
		public void HandleJump()
		{
			var rb = SM.info.rb;
			//rb.AddForce(Vector2.up * this.jumpForce);
			rb.velocity = new Vector2(rb.velocity.x, this.jumpSpeed);
		}

		public void HandleWallSlide()
		{
			var rb = SM.info.rb;
			// eraze the xVelocity during wall slide
			if (this.inpVel.y < 0f)
				rb.velocity = new Vector2(0f, rb.velocity.y * 1f);
			else
				rb.velocity = new Vector2(0f, rb.velocity.y * this.ySpeedWallMultiplier); // if pressed nothing
		}
		public void HandleWallJump()
		{
			var rb = SM.info.rb;
			rb.velocity = new Vector2(-this.getXFacingDir, +1.5f) * this.wallJumpSpeed;
		}

		public void HandleXAttackMovement()
		{
			var rb = SM.info.rb;
			rb.velocity = new Vector2(this.basicAttackSpeed * this.getXFacingDir, rb.velocity.y);
		}
		#endregion

		private void DrawCollisionDetection(Vector2 center)
		{
			DRAW.col = Color.red;
			DRAW.dt = Time.deltaTime;
			DRAW.ARROW(center, center + new Vector2(0, -this.groundCheckDist), t: 1f);

			Vector2 a = center + Vector2.up * this.wallCheckRaysApartDist,
					b = center - Vector2.up * this.wallCheckRaysApartDist;
			DRAW.ARROW(a, a + new Vector2(this.wallCheckDist * this.getXFacingDir, 0f), t: 1f);
			DRAW.ARROW(b, b + new Vector2(this.wallCheckDist * this.getXFacingDir, 0f), t: 1f);
		}
	}
}
