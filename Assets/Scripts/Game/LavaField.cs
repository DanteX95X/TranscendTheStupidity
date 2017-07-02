using UnityEngine;
using System.Collections;


namespace Assets.Scripts.Game
{
	public class LavaField : Field 
	{
		[SerializeField]
		int directDamage = 3;

		[SerializeField]
		int indirectDamage = 2;

		[SerializeField]
		GameObject explosion = null;

		public override float TraversalCost()
		{
			if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Level>().DidVolcanoErupt[transform.position])
				return 0;
			return 20;
		}

		public override void TriggerTile(Lemming lemming)
		{
			if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Level>().DidVolcanoErupt[transform.position])
				return;

			Instantiate(explosion, transform.position, transform.rotation);
			GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);

			lemming.Health -= directDamage;
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Level>().DidVolcanoErupt[transform.position] = true;
			foreach(GameObject collateralDamage in GameObject.FindGameObjectsWithTag("Lemming"))
			{
				if((collateralDamage.transform.position - transform.position).magnitude < 2.1)
				{
					collateralDamage.GetComponent<Lemming>().Health -= indirectDamage;
				}
			}
		}
	}
}