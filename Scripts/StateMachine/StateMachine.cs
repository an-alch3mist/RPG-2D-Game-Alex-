using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPACE_UTIL;

namespace SPACE_RPG2D
{
	// >> Dynamic
	public enum StateType
	{
		player_idle,
		player_move,

		player_air,
		player_jump,
		player_fall,

		player_wallslide,
		player_walljump,

		player_dash,
		player_basicattack,

		// i, f >>
		player_yvel_float,
		player_comboattack_index,
		// << i, f
		none,
	}

	public class PlayerInfo
	{
		public GameObject obj;
		public Animator animator;
		public Rigidbody2D rb;
		public Player player;
	}
	// << Dynamic


	public class StateMachine
	{
		public string name;
		EntityState CurrentState;
		public Dictionary<StateType, EntityState> MAP_STATE;
		public PlayerInfo info;

		public StateMachine(string name, PlayerInfo playerInfo)
		{
			this.name = name + "StateMachine";
			this.MAP_STATE = new Dictionary<StateType, EntityState>();
			this.info = playerInfo;
		}

		// called externally when state is changed
		public void GoTo(StateType stateType)
		{
			EntityState NewState = this.MAP_STATE[stateType];
			if (NewState == this.CurrentState) // same as current state
				return;

			this.CurrentState? // if not null
				.Exit();
			NewState.Enter();
			this.CurrentState = NewState;
		}

		// called externaly when revisit of state required with different parameter
		Coroutine routine_ref;
		public void GoToRevisit()
		{
			if (this.CurrentState == null) // if null: exit
				return;
			
			if(routine_ref != null)
				INITManager.Ins.StopCoroutine(routine_ref);
			routine_ref = INITManager.Ins.StartCoroutine(routine());
		}

		IEnumerator routine()
		{
			this.CurrentState.Exit();
			yield return new WaitForEndOfFrame();
			this.CurrentState.Enter();
		}



		// called externally every frame
		public void UpdateCurrentState()
		{
			this.CurrentState? // if not null
				.Update(); 
		}
	}


	[System.Serializable]
	public abstract class EntityState
	{
		public string id; // this id is also used inside animator.SetBool()
		public StateType stateType;
		public StateMachine SM;

		public StateType blendStateType;
		public string blend_id;

		protected EntityState(StateType stateType, StateMachine stateMachine, StateType blendStateType = StateType.none)
		{
			this.stateType = stateType;
			this.id = stateType.ToString();
			this.SM = stateMachine;

			stateMachine.MAP_STATE[stateType] = this; // ref in MAP_STATE

			#region blendTree
			this.blendStateType = blendStateType;
			this.blend_id = blendStateType.ToString();
			#endregion
		}

		protected EntityState() { }

		#region ad
		public override string ToString()
		{
			//return base.ToString();
			return $"(EntityState) stateType: {this.stateType}, stateMachine ref: {this.SM.name}";
		}
		#endregion

		public virtual void Enter()
		{
			Debug.Log($"state id: {this.id} Enter()");

			// must be animator != null
			if (this.blendStateType == StateType.none)
				SM.info.animator.SetBool(this.id, true); 
			else
				SM.info.animator.SetBool(this.blend_id, true);
		}

		public virtual void Update()
		{
			// Debug.Log($"state id: {this.id} Update()");
		}

		public virtual void Exit()
		{
			Debug.Log($"state id: {this.id} Exit()");

			// must be animator != null
			if (this.blendStateType == StateType.none)
				SM.info.animator.SetBool(this.id, false); 
			else
				SM.info.animator.SetBool(this.blend_id, false);
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
