using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities;

namespace Assets.Scripts.Game
{
	public class ForestField : Field
	{
		public override float TraversalCost()
		{
			return PathFinding.IMPASSABILITY_THRESHOLD;
		} 

	}
}
