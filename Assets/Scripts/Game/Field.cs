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
		List<GameObject> tiles = null;

		public Dictionary<Vector2, Field> Neighbours
		{
			get { return neighbours; }
		}

		public int Group
		{
			set 
			{ 
				group = value;
				gameObject.tag = "Group" + value; 
			}
		}

		void Start () 
		{
			neighbours = new Dictionary<Vector2, Field>();
			Group = group;
		}

		void Update () 
		{
			if(Input.GetButtonDown("Jump") && group == 1)
			{
				GameObject newTile = Instantiate(tiles[0], transform.position, transform.rotation) as GameObject;
				newTile.transform.parent = GameObject.Find("Grid").transform;
				Destroy(gameObject);
			}
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

		public virtual void TriggerTile()
		{
			Debug.Log("Triggered");
		}
	}
}