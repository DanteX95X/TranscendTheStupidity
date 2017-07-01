using UnityEngine;
using System.Collections;
using Assets.Scripts.Game;
using Assets.Scripts.Utilities;
using System.Collections.Generic;

namespace Assets.Scripts.Game
{
	public class Lemming : MonoBehaviour 
	{
		Field currentField;
		Level currentLevel;

		[SerializeField]
		Vector2 goal = new Vector2(3,3);

		[SerializeField]
		float speed = 0.001f;

		Vector3 velocity;

		float timeCounter;

		[SerializeField]
		float rotationTime = 1;

		[SerializeField]
		float angle = 0;

		void Start () 
		{
			currentField = null;
			currentLevel = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Level>();
			timeCounter = 0;
		}

		void Update()
		{
			Vector3 approximatePosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
			if (currentField == null || (currentField.transform.position != currentLevel.Fields[approximatePosition].transform.position && (transform.position - currentLevel.Fields[approximatePosition].transform.position).magnitude < 0.1))
			{
				currentField = currentLevel.Fields[approximatePosition];
				List<Field> path = PathFinding.AStar(currentField, currentLevel.Fields[goal], PathFinding.EuclideanHeuristic);
				if (path.Count > 1)
				{
					velocity = (path[1].transform.position - transform.position).normalized;
					if(velocity.x > 0)
						GetComponent<SpriteRenderer>().flipX = false;
					else if(velocity.x < 0)
						GetComponent<SpriteRenderer>().flipX = true;
				} 
				else
				{
					Destroy(gameObject);
				}
			}

			transform.position += velocity * speed * Time.deltaTime;

			timeCounter += Time.deltaTime;

			if (timeCounter > rotationTime)
			{
				angle = -angle;
				timeCounter = 0;
			}

			transform.Rotate(new Vector3(0,0,1), angle);

			
		}
	}
}
