using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
	public GameObject fuelCellPrefab;

	private const float nextPointThreshold = 0.03f;

	[SerializeField]
	private float moveSpeed = 0.65f;

	List<GameObject> tiles = new List<GameObject>(4);

	Vector3 nextTileFirstPoint;

	List<Vector3> points = new List<Vector3>();

	private GameObject tileManager;

	public AudioSource cartAudio;
	public AudioSource cartIgnition;
	public AudioSource cartStop;

	public Transform needle;

	public float cartFuel;
	public float cartFuelBurnRate;

	private bool started = false;
	private bool cartRunning = false;
	public float cartStartDelay;

	void Start()
	{
		tileManager = GameObject.Find("TileManager");

		GameObject startingPlatform = GameObject.Find("InitialPlatform");
		TileController startingTileController = startingPlatform.GetComponent<TileController>();

		tiles.Add(startingPlatform);

		GameObject PointParent = GetChildObjectWithTag(startingPlatform.transform, "Points");

		//Add all of the points of the new tile to the movement queue
		foreach (Transform point in PointParent.transform)
		{
			points.Add(point.position);
		}

		nextTileFirstPoint = PointParent.transform.GetChild(0).position;
	}

	void FixedUpdate()
	{
		if (!started)
		{
			if (cartStartDelay <= 0)
			{
				StartCart();
				cartStartDelay = 0f;
				started = true;
			}
			else
			{
				cartStartDelay -= Time.deltaTime;
			}
		}


		if (cartRunning)
		{
			Move();
		}
		CheckPoint();

		if (cartRunning)
		{
			cartFuel -= cartFuelBurnRate * Time.deltaTime;

			if (cartFuel <= 0f)
			{
				cartFuel = 0f;
				StopCart();
			}
		}

		float targetRot = cartFuel * 320f;

		//needle.Rotate(targetRot - needle.rotation.eulerAngles.x, 0f, 0f, Space.Self);
		//needle.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		needle.rotation = Quaternion.Euler(new Vector3(-targetRot, needle.rotation.eulerAngles.y, needle.rotation.eulerAngles.z));
		//Debug.Log("Target: " + targetRot + " Actual: " + needle.rotation.eulerAngles.x);
		//Debug.Log(" Actual: " + needle.rotation.eulerAngles);
	}

	void StartCart()
	{
		cartIgnition.Play();
		cartAudio.Play();
		cartRunning = true;
	}

	void StopCart()
	{
		cartStop.Play();
		cartAudio.Stop();
		cartRunning = false;
	}

	void Move()
	{
		transform.LookAt(points[0]);

		transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

	void CheckPoint()
	{
		Vector3 currentPoint = transform.position;

		if (Vector3.Distance(transform.position, points[0]) < nextPointThreshold)
		{
			points.RemoveAt(0);

			//This only needs to be checked if we're at a new point, not in transit
			if (Vector3.Distance(transform.position, nextTileFirstPoint) < nextPointThreshold)
			{
				SpawnTile();
			}
		}
	}

	void SpawnTile()
	{
		//Identify new parent tile
		TileController currentTileController = tiles[0].GetComponent<TileController>();

		//Spawn tile
		GameObject newTile = currentTileController.SpawnOffOfThisTile()[0];
		GameObject PointParent = GetChildObjectWithTag(newTile.transform, "Points");

		//Add all of the points of the new tile to the movement queue
		foreach(Transform point in PointParent.transform)
		{
			points.Add(point.position);
		}

		//Destroy old tiles and shift arraylist
		if(tiles.Count >= 4)
		{
			Destroy(tiles[3]);
			tiles.RemoveAt(3);	
		}
		tiles.Insert(0, newTile);

		//gets the first child, the list of points, then gets the first points (child) and gets its position;
		nextTileFirstPoint = PointParent.transform.GetChild(0).position;

		if (!newTile.GetComponent<TileController>().isFuelingTile)
		{
			//Add fuel cells
			int numCells = Random.Range(1, 5);
			List<Vector3> cellPoints = new List<Vector3>();
			List<Vector3> spawnCellPoints = new List<Vector3>();

			GameObject FuelCellPointParent = GetChildObjectWithTag(newTile.transform, "FuelCellPoints");

			foreach (Transform cellPoint in FuelCellPointParent.transform)
			{
				cellPoints.Add(cellPoint.position);
			}

			for (int i = 1; i <= numCells; i++)
			{
				bool pointAdded = false;

				while (!pointAdded)
				{
					int cellPoint = Random.Range(0, cellPoints.Count);

					if (!spawnCellPoints.Contains(cellPoints[cellPoint]))
					{
						spawnCellPoints.Add(cellPoints[cellPoint]);
						pointAdded = true;
					}
				}
			}

			foreach (Vector3 point in spawnCellPoints)
			{
				Instantiate(fuelCellPrefab, point, Quaternion.Euler(0, 0, 0));
			}
		}
	}

	public GameObject GetChildObjectWithTag(Transform Parent, string Tag)
    {
        for (int i = 0; i < Parent.childCount; i++)
        {
            Transform Child = Parent.GetChild(i);
            if (Child.tag == Tag)
            {
                return Child.gameObject;
            }
            if (Child.childCount > 0)
            {
                GetChildObjectWithTag(Child, Tag);
            }
        }

        return null;
    }
}