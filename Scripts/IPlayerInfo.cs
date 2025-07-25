using UnityEngine;
using System.Collections;

namespace SPACE_RPG2D
{
	public interface IComponentInfo
	{
		GameObject Obj();
		Rigidbody2D rb();
		Animator animator();
	} 
}
