using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
	public class TileButton : MonoBehaviour 
	{
		[SerializeField]
		GameObject tile = null;

		void Start () 
		{
			GetComponent<Button>().image.sprite = tile.GetComponent<SpriteRenderer>().sprite;
		}
		void Update () 
		{
			
		}

		public void ChangeGroupToTile(int group)
		{
			foreach (GameObject field in GameObject.FindGameObjectsWithTag("Group" + group))
			{
				GameObject newTile = Instantiate(tile, field.transform.position, field.transform.rotation) as GameObject;
				newTile.transform.parent = GameObject.Find("Grid").transform;
				newTile.GetComponent<Field>().Group = group;
				Destroy(field);
			}
		}
	}
}