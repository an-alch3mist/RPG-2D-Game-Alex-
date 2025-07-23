using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class StateMachine
	{
		public EntityState CurrentState;
		// called externally when state is changed
		public void Goto(EntityState NewState)
		{
			this.CurrentState?.Exit(); // if not null

			NewState.Enter();
			this.CurrentState = NewState;
		}

		// called externally every frame
		public void UpdateCurrentState()
		{
			this.CurrentState?.Update(); // if not null
		}
	}

	public abstract class EntityState
	{
		public string name = "state-name"; // this id is also used inside animator.SetBool()
		public string id = "state-id"; // this id is also used inside animator.SetBool()

		public GameObject gameObject;
		public Animator animator;
		public Rigidbody2D rb;

		protected EntityState(string id, GameObject gameObject, GameObject animator, GameObject rb)
		{
			this.name = this.id = id;
			this.gameObject = gameObject;
			this.animator = animator.GetComponent<Animator>();
			this.rb = animator.GetComponent<Rigidbody2D>();
		}

		public virtual void Enter()
		{
			Debug.Log($"state id: {this.id} enter");
		}

		public virtual void Update()
		{
			Debug.Log($"state id: {this.id} update");
		}

		public virtual void Exit()
		{
			Debug.Log($"state id: {this.id} exit");
		}
	}

	// example EntityState >>
	public class IdleState : EntityState
	{
		public IdleState(string id, GameObject gameObject, GameObject animator, GameObject rb) 
				  : base(id, gameObject, animator, rb)
		{

		}

		public override void Enter()
		{
			base.Enter();
			this.animator.SetBool(id, true);
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Exit()
		{
			base.Exit();
			this.animator.SetBool(id, false);
		}

	}
	// << example EntityState
}
