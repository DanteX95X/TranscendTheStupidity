using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Game
{
	public class BeartrapField : Field 
	{
		[SerializeField]
		int damage = 1;

		public override float TraversalCost()
		{
			return 1;
		}

		public override void TriggerTile(Lemming lemming)
		{
			GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
			lemming.Health -= damage;
		}
	}
}