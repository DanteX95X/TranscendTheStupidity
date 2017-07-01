using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Scripts.Game;

namespace Assets.Scripts.Utilities
{
	public class PathFinding
	{
		public static readonly int IMPASSABILITY_THRESHOLD = 10000;

		public delegate float Heuristic(Field currentField, Field goalField);

		public static List<Field> AStar (Field startField, Field goalField, Heuristic heuristic)
		{
			if (startField == goalField)
			{
				//Debug.Log ("Could not calculate path between the same fields!");
				List<Field> result = new List<Field>();
				result.Add (startField);
				return result;
			}

			Field currentField = startField;

			HashSet<Field> visited = new HashSet<Field> ();
			HashSet<Field> opened = new HashSet<Field> (); 

			Dictionary<Field, float> costsFromStart = new Dictionary<Field, float> ();
			Dictionary<Field, Field> cameFrom = new Dictionary<Field, Field>();

			PriorityQueue<Field> frontier = new PriorityQueue<Field> ();

			frontier.Push (currentField, heuristic(currentField, goalField));
			opened.Add (currentField);
			costsFromStart [currentField] = 0;

			while (frontier.Count > 1)
			{
				currentField = frontier.Pop();

				if (opened.Contains (currentField) == true)
				{
					opened.Remove (currentField);
				}
				else
				{
					continue;
				}

				visited.Add (currentField);

				if (currentField == goalField)
				{
					//Debug.Log ("goalField reached!"); Nobody cares!
					return GetPath(cameFrom, goalField, startField);
				}

				foreach (Field neighbour in currentField.Neighbours.Values)
				{

					if (!visited.Contains(neighbour))
					{
						float temporaryCostFromStart = costsFromStart[currentField] + CalculateCost(currentField, neighbour);//1;

						if ((! opened.Contains (neighbour)) || (temporaryCostFromStart < costsFromStart [neighbour]))
						{
							if (! opened.Contains (neighbour))
							{
								opened.Add (neighbour);
							}

							costsFromStart [neighbour] = temporaryCostFromStart;
							float tentativeCost = costsFromStart [neighbour] + heuristic (neighbour, goalField);
							frontier.Push (neighbour, tentativeCost);
							cameFrom [neighbour] = currentField;
						}
					}
				}
			}

			Debug.Log ("Goal field is unreachable!");
			return new List<Field>();
		}

		public static List<Field> GetPath(Dictionary<Field, Field> cameFrom, Field goalField, Field startField)
		{
			List<Field> result = new List<Field>();
			result.Add (goalField);
			Field previousField;
			Field currentField = goalField;

			do
			{
				cameFrom.TryGetValue (currentField, out previousField);

				if(CalculateCost(previousField, currentField) >= IMPASSABILITY_THRESHOLD)
				{
					result.Clear();
					//Debug.Log("Impassable");
					//return result;
				}

				result.Add (previousField);
				currentField = previousField;

			}
			while(currentField != startField);

			result.Reverse ();

			/*for (int i = 0; i < result.Count - 1; ++i)
			{
				if (CalculateCost(result[i], result[i + 1]) >= IMPASSABILITY_THRESHOLD)
				{
					result.Clear();
				}
			}*/

			return result;
		}

		public static float ManhattanHeuristic(Field currentField, Field goalField)
		{
			Vector2 differenceVector = goalField.transform.position - currentField.transform.position;
			int heuristic = Math.Abs ((int)differenceVector.x) + Math.Abs ((int)differenceVector.y);
			return heuristic;
		}

		public static float EmptyHeuristic(Field current, Field goal)
		{
			return 0;
		}

		public static float EuclideanHeuristic(Field currentField, Field goalField)
		{
				return Mathf.Sqrt((currentField.transform.position.x - goalField.transform.position.x)* (currentField.transform.position.x - goalField.transform.position.x) + (currentField.transform.position.y - goalField.transform.position.y)*(currentField.transform.position.y - goalField.transform.position.y));
		}

		public static float CalculateCost(Field current, Field next)
		{
			float cost = ManhattanHeuristic(current, next); //EuclideanHeuristic(current, next);
			cost += next.TraversalCost();
			return cost;
		}

	}
}
