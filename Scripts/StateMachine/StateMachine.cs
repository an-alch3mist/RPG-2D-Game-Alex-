using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class StateMachine
	{
		EntityState CurrentState;
		// called externally when state is changed
		public void GoTo(EntityState NewState)
		{
			this.CurrentState? // if not null
				.Exit();

			NewState.Enter();
			this.CurrentState = NewState;
		}

		// called externally every frame
		public void UpdateCurrentState()
		{
			this.CurrentState? // if not null
				.Update(); 
		}

		public Dictionary<StateType, EntityState> MAP_STATE = new Dictionary<StateType, EntityState>();
	}

	public enum StateType
	{
		player_idle,
		player_move,
		player_jump,
	}

	public abstract class EntityState
	{
		public string id; // this id is also used inside animator.SetBool()
		public StateType stateType;
		public StateMachine stateMachine;
		public IComponentInfo info; // gameObject, rb, animator ....

		protected EntityState(StateType stateType, StateMachine stateMachine, IComponentInfo info)
		{
			this.stateType = stateType;
			this.id = stateType.ToString();
			this.stateMachine = stateMachine;
			this.info = info;
		}

		protected EntityState()
		{

		}

		public virtual void Enter()
		{
			Debug.Log($"state id: {this.id} Enter()");
		}

		public virtual void Update()
		{
			Debug.Log($"state id: {this.id} Update()");
		}

		public virtual void Exit()
		{
			Debug.Log($"state id: {this.id} Exit()");
		}
	}

	#region example
	// example EntityState >>
	/*
	public class AState : EntityState
	{
		public AState(string id, GameObject gameObject, GameObject animator, GameObject rb)
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
	*/
	// << example EntityState 
	#endregion
}
