using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SPACE_UTIL;

namespace SPACE_RPG2D
{
	public class DEBUG_Check : MonoBehaviour
	{
		private void Update()
		{
			if(INPUT.M.InstantDown(0))
			{
				StopAllCoroutines();
				StartCoroutine(STIMULATE());
			}
		}

		IEnumerator STIMULATE()
		{
			#region frame_rate
			QualitySettings.vSyncCount = 1;
			yield return null;
			#endregion

			//this.Check_map_refine();
			//this.Check_gameObjectQuery();
		}


		void Check_map_refine()
		{
			List<int> LIST = new List<int>()
			{
				0, 1, 4, 10, 100, 1000, -1, 0, 1, 2,
			};

			LOG.SaveLog(LIST.map(elem => elem).ToTable(name: "LIST<int> original"));
			LOG.SaveLog(LIST.map(elem => elem * Mathf.PI).ToTable(name: "LIST<float> from map"));
			LOG.SaveLog(LIST.refine((elem, i) => (i % 3 == 0)).map((elem, i) => elem ).ToTable(name: "LIST<float> from refine + map"));
		}

		[SerializeField] string Query = "";
		[SerializeField] GameObject Obj_ansc;
		void Check_gameObjectQuery()
		{
			Debug.Log(this.Obj_ansc.Query(this.Query));
		}

	} 
}
