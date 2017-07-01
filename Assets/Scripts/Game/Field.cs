using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
	public class Field : MonoBehaviour 
	{
		Dictionary<Vector2, Field> neighbours;

		public Dictionary<Vector2, Field> Neighbours
		{
			get { return neighbours; }
		}

		void Start () 
		{
			neighbours = new Dictionary<Vector2, Field>();
		}

		void Update () 
		{
		
		}

		public void SetNeighbours(Dictionary<Vector2, Field> fields)
		{
			List<Vector3> displacements = new List<Vector3>(){new Vector3(1,0), new Vector3(1, -1), new Vector3(0, -1),  new Vector3(-1, -1), new Vector3(-1, 0), new Vector3(-1, 1), new Vector3(0, 1), new Vector3(1, 1)}; 

			foreach(Vector3 displacement in displacements)
			{
				Field temp = null;
				fields.TryGetValue(transform.position + displacement, out temp);
				if(temp != null)
					neighbours[displacement] = temp;
			}
		}
	}
}