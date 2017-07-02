using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {


	void Start ()
	{
		StartCoroutine(Boom());
	}

	IEnumerator Boom()
	{
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}
}
