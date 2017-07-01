using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
	public class Field : MonoBehaviour 
	{
		Dictionary<Vector2, Field> neighbours;

		[SerializeField]
		int group = 0;

		[SerializeField]
		Sprite[] groupSprites = new Sprite[6];

		public Dictionary<Vector2, Field> Neighbours
		{
			get { return neighbours; }
		}

		public int Group {
			set 
			{ 
				group = value;
				if (group >= 0)
				{
					gameObject.tag = "Group" + value;
					foreach (Transform child in transform)
						child.GetComponent<SpriteRenderer>().sprite = groupSprites[group];
				}
			}
		}

		void Start () 
		{
			neighbours = new Dictionary<Vector2, Field>();
			Group = group;
		}

		void Update () 
		{
		}

		public void SetNeighbours(Dictionary<Vector2, Field> fields)
		{
			List<Vector3> displacements = new List<Vector3>(){new Vector3(1,0), /*new Vector3(1, -1),*/ new Vector3(0, -1),  /*new Vector3(-1, -1),*/ new Vector3(-1, 0), /*new Vector3(-1, 1),*/ new Vector3(0, 1), /*new Vector3(1, 1)*/}; 

			foreach(Vector3 displacement in displacements)
			{
				Field temp = null;
				fields.TryGetValue(transform.position + displacement, out temp);
				if(temp != null)
					neighbours[displacement] = temp;
			}
		}

		public virtual float TraversalCost()
		{
			return 0;
		}

		public void OnDestroy()
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Level>().RebuildGrid();
			foreach(GameObject lemming in GameObject.FindGameObjectsWithTag("Lemming"))
			{
				lemming.GetComponent<Lemming>().UpdatePath();
			}
		}

		public virtual void TriggerTile(Lemming lemming)
		{
		}
	}
}