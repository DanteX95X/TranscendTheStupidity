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
			Vector3 approximatedPosition = ApproximatePosition();
			if (currentField == null || (currentField.transform.position != currentLevel.Fields[approximatedPosition].transform.position && (transform.position - currentLevel.Fields[ApproximatePosition()].transform.position).magnitude < 0.1))
			{
				UpdatePath();
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

		public void UpdatePath()
		{
			currentField = currentLevel.Fields[ApproximatePosition()];
			List<Field> path = PathFinding.AStar(currentField, currentLevel.Fields[goal], PathFinding.ManhattanHeuristic);
			if (path.Count > 1)
			{
				velocity = (path[1].transform.position - transform.position).normalized;
				if(velocity.x > 0)
					GetComponent<SpriteRenderer>().flipX = false;
				else if(velocity.x < 0)
					GetComponent<SpriteRenderer>().flipX = true;
			} 
			else if(path[0].transform.position == new Vector3(goal.x, goal.y, path[0].transform.position.z))
			{
				currentLevel.LemmingsEscape();
				Destroy(gameObject);
			}
			else
			{
				velocity = new Vector3(0,0,0);
			}
		}

		public Vector3 ApproximatePosition()
		{
			return new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), transform.position.z);
		}
	}
}
