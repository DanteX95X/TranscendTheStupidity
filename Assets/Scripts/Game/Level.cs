using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
	public class Level : MonoBehaviour 
	{
		Dictionary<Vector2, Field> fields;

		[SerializeField]
		GameObject lemmingPrefab = null;

		[SerializeField]
		Text uiText = null;

		[SerializeField]
		float spawnTime = 1;

		[SerializeField]
		Vector3 spawnPoint = new Vector3(0,0,0);

		float timeCounter;

		[SerializeField]
		int lemmingCap = 0;

		int lemmingsEscapedQuantity;

		public Dictionary<Vector2, Field> Fields
		{
			get { return fields; }
		}

		void Start()
		{
			timeCounter = 0;
			
			fields = new Dictionary<Vector2, Field>();
			RebuildGrid();

			lemmingsEscapedQuantity = 0;
			UpdateUIText();
		}

		void Update () 
		{
			timeCounter += Time.deltaTime;
			if(timeCounter > spawnTime)
			{
				timeCounter = 0;
				Instantiate(lemmingPrefab, spawnPoint, Quaternion.identity);
			}

			if(lemmingsEscapedQuantity >= lemmingCap)
				Debug.Log("Game Over");
		}

		void UpdateUIText()
		{
			uiText.text = "Lemmings Escaped " + lemmingsEscapedQuantity + "/" + lemmingCap;
		}

		public void LemmingsEscape()
		{
			++lemmingsEscapedQuantity;
			UpdateUIText();
		}

		public void RebuildGrid()
		{
			GameObject grid = GameObject.Find("Grid");
			foreach (Transform field in grid.transform)
			{
				fields[field.position] = field.GetComponent<Field>();
			}

			foreach (Transform field in grid.transform)
			{
				field.gameObject.GetComponent<Field>().SetNeighbours(fields);
			}
		}
	}
}