using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{

	float moveSpeed = 5f;

	List<GameObject> tiles = new List<GameObject>(4);

	Vector3 nextTileFirstPoint = Vector3.negativeInfinity;

	List<Vector3> points = new List<Vector3>();

    void Start()
    {
        
    }

    void FixedUpdate()
    {
		Move();
		CheckPoint();
    }

	void Move()
	{
		transform.LookAt(points[0]);
		//move to points[0]
	}

	void CheckPoint()
	{
		Vector3 currentPoint = transform.position;

		if (currentPoint == points[0])
		{
			points.RemoveAt(0);

			if (currentPoint == nextTileFirstPoint)
			{
				SpawnTile();
			}

		}

	}

	void SpawnTile()
	{
		//Do tile spawing

		GameObject newTile;

		tiles.RemoveAt(3);

		tiles.Insert(0, newTile);

		//gets the first child, the list of points, then gets the first points (child) and gets its position;
		nextTileFirstPoint = newTile.transform.GetChild(0).GetChild(0).transform.position;

		//get the tiles points add them to the points list
	}

}