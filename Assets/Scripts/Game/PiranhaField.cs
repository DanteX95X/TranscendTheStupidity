using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Game
{
	public class PiranhaField : Field 
	{
		[SerializeField]
		int damage = 3;


		public override float TraversalCost()
		{
			return 2;
		}

		public override void TriggerTile(Lemming lemming)
		{
			GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
			lemming.Health -= damage;
		}
	}
}