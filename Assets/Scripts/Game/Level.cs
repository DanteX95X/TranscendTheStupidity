﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.Game
{
	public class Level : MonoBehaviour 
	{
		Dictionary<Vector2, Field> fields;

		[SerializeField]
		GameObject lemmingPrefab = null;

		[SerializeField]
		float spawnTime = 1;

		[SerializeField]
		Vector3 spawnPoint = new Vector3(0,0,0);

		float timeCounter;

		public Dictionary<Vector2, Field> Fields
		{
			get { return fields; }
		}


		void Start () 
		{
			timeCounter = 0;
			
			fields = new Dictionary<Vector2, Field>();
			GameObject grid = GameObject.Find("Grid");
			foreach(Transform field in grid.transform)
			{
				fields[field.position] = field.GetComponent<Field>();
			}

			foreach(Transform field in grid.transform)
			{
				field.gameObject.GetComponent<Field>().SetNeighbours(fields);
				Debug.Log(field.GetComponent<Field>().Neighbours.Values.Count + " " + field.position);
			}

			var list = PathFinding.AStar(fields[new Vector2(0,0)], fields[new Vector2(3,2)], PathFinding.EuclideanHeuristic);
			string path = "";
			foreach(var node in list)
				path += node.transform.position + " ";
			Debug.Log(path);
		}

		void Update () 
		{
			timeCounter += Time.deltaTime;
			if(timeCounter > spawnTime)
			{
				timeCounter = 0;
				Instantiate(lemmingPrefab, spawnPoint, Quaternion.identity);
			}
		}
	}
}