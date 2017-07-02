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
		Text winConditionIndicator = null;

		[SerializeField]
		Text messageSystem = null;

		Dictionary<Vector3, bool> didVulcanoErupt;

		[SerializeField]
		float spawnTime = 1;

		[SerializeField]
		Vector3 spawnPoint = new Vector3(0,0,0);

		float timeCounter;

		[SerializeField]
		int lemmingCap = 0;

		[SerializeField]
		int lemmingWave = 0;

		int lemmingsEscapedQuantity;

		public Dictionary<Vector2, Field> Fields
		{
			get { return fields; }
		}

		public Dictionary<Vector3, bool> DidVolcanoErupt
		{
			get { return didVulcanoErupt; }
			set { didVulcanoErupt = value; }
		}

		public int LemmingWave
		{
			get { return lemmingWave; }
			set { lemmingWave = value; }
		}

		void Start()
		{
			timeCounter = spawnTime+1;
			didVulcanoErupt = new Dictionary<Vector3, bool>();
			
			fields = new Dictionary<Vector2, Field>();
			RebuildGrid();

			foreach (Transform field in GameObject.Find("Grid").transform)
			{
				didVulcanoErupt[field.position] = false;
			}

			lemmingsEscapedQuantity = 0;
			UpdateUIText();
		}

		void Update()
		{
			timeCounter += Time.deltaTime;
			if (timeCounter > spawnTime && lemmingWave > 0)
			{
				timeCounter = 0;
				Instantiate(lemmingPrefab, spawnPoint, Quaternion.identity);
				--lemmingWave;
				winConditionIndicator.text = "Lemmings left: " + lemmingWave;
			}

			if (lemmingsEscapedQuantity >= lemmingCap)
			{
				Debug.Log("Game Over");
				messageSystem.gameObject.SetActive(true);
				messageSystem.text = "Game Over!";
				Destroy(GameObject.Find("Grid"));
				foreach(GameObject lemming in GameObject.FindGameObjectsWithTag("Lemming"))
				{
					Destroy(lemming.gameObject);
				}
			} 
			else if (lemmingWave <= 0 && GameObject.FindGameObjectsWithTag("Lemming").Length <= 0)
			{
				messageSystem.gameObject.SetActive(true);
				Debug.Log("You won");
				messageSystem.text = "You Won!";
				Destroy(GameObject.Find("Grid"));
				foreach(GameObject lemming in GameObject.FindGameObjectsWithTag("Lemming"))
				{
					Destroy(lemming.gameObject);
				}
			}
		}

		void UpdateUIText()
		{
			uiText.text = "Lemmings Escaped: " + lemmingsEscapedQuantity + "/" + lemmingCap;
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