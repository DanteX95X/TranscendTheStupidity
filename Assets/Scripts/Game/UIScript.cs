using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Game
{
	public class UIScript : MonoBehaviour 
	{

		[SerializeField]
		GameObject[] ui = new GameObject[6];

		void Start () 
		{
		
		}

		void Update () 
		{
		
		}

		public void Trigger(int group)
		{
			foreach(GameObject temp in ui)
				temp.SetActive(false);
			ui[group].SetActive(true);
		}
	}
}