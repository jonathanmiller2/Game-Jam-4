using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{

	[SerializeField]
	public List<GameObject> tiles;
	[SerializeField]
	public List<GameObject> fuelingTiles;
	public int frequencyOfFuelingTile;

	private int spawnedTiles = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn(Vector3 Position, Vector3 Center)
    {
		GameObject nextTile;
		Vector3 Direction = Vector3.Normalize(Center - Position);

		if (spawnedTiles % frequencyOfFuelingTile == 0)
		{
			int i = Random.Range(0, fuelingTiles.Count);

			nextTile = Instantiate(fuelingTiles[i], Position, Quaternion.identity);
			nextTile.transform.forward = Direction;

			nextTile.GetComponent<TileController>().isFuelingTile = true;
		}
		else
		{
			int i = Random.Range(0, tiles.Count);
			
			nextTile = Instantiate(tiles[i], Position, Quaternion.identity);
			nextTile.transform.forward = Direction;
		}

		spawnedTiles += 1;

		return nextTile;
    }
}
