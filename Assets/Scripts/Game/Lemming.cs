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

		[SerializeField]
		int health = 3;

		[SerializeField]
		AudioClip[] clips = new AudioClip[2];


		public int Health 
		{
			get { return health; }
			set 
			{ 
				health = value;
				GetComponentInChildren<TextMesh>().text = "" + health; 
			}
		}

		void Start () 
		{
			currentField = null;
			currentLevel = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Level>();
			timeCounter = 0;
			Health = health;
		}

		void Update()
		{
			if (health <= 0)
			{
				Destroy(gameObject);
			}

			Vector3 approximatedPosition = ApproximatePosition();
			if (currentField == null || (currentField.transform.position != currentLevel.Fields[approximatedPosition].transform.position && (transform.position - currentLevel.Fields[approximatedPosition].transform.position).magnitude < 0.1))
			{
				transform.position = currentLevel.Fields[approximatedPosition].transform.position;
				UpdatePath();
				currentField.TriggerTile(this);
			}

			transform.position += velocity * speed * Time.deltaTime;

			timeCounter += Time.deltaTime;

			if (timeCounter > rotationTime)
			{
				angle = -angle;
				timeCounter = 0;
			}

			transform.Rotate(new Vector3(0,0,1), angle * Time.deltaTime);

			
		}

		public void UpdatePath()
		{
			currentField = currentLevel.Fields[ApproximatePosition()];
			List<Field> path = PathFinding.AStar(currentField, currentLevel.Fields[goal], PathFinding.EmptyHeuristic/*ManhattanHeuristic*/);
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
				GetComponent<AudioSource>().PlayOneShot(clips[1]);
				StartCoroutine(CommitSuicide());
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

		IEnumerator CommitSuicide()
		{
			yield return new WaitForSeconds(0.5f);
			Destroy(gameObject);
		}
	}
}
