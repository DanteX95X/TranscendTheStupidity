using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Utilities
{
	public class CameraPosition : MonoBehaviour 
	{

		void Start () 
		{
			GameObject grid = GameObject.Find("Grid");
			Vector2 minVector = new Vector2(int.MaxValue, int.MaxValue);
			Vector2 maxVector = new Vector2(int.MinValue, int.MinValue);
			foreach(Transform child in grid.transform)
			{
				if(child.position.x < minVector.x)
					minVector.x = child.position.x;
				else if(child.position.x > maxVector.x)
					maxVector.x = child.position.x;

				if(child.position.y < minVector.y)
					minVector.y = child.position.y;
				else if(child.position.y > maxVector.y)
					maxVector.y = child.position.y; 
			}

			Vector2[] positions = new Vector2[]{minVector, maxVector};
			SetCamera(positions);
		}
		
		void Update () 
		{
		
		}

		void SetCamera(Vector2[] positions)
		{
				Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
				Vector3 desiredPosition = new Vector3((positions[0].x + positions[1].x)/2, (positions[0].y + positions[1].y)/2, -10);
				mainCamera.transform.position = desiredPosition;


				Vector3 desiredLocalPosition = mainCamera.transform.InverseTransformPoint(desiredPosition);
				float size = 0;
				foreach (Vector3 position in positions)
				{
					Vector3 targetLocalPosition = mainCamera.transform.InverseTransformPoint(position);
					Vector3 desiredPositionToTarget = targetLocalPosition - desiredLocalPosition;

					size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.y));
					size = Mathf.Max(size, Mathf.Abs(desiredPositionToTarget.x) / mainCamera.aspect);
				}
				size += 1;
				mainCamera.orthographicSize = size;
		}
	}
}
