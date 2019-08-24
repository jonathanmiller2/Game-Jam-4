using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> tiles = new List<GameObject>(3);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(Vector3 Position, Vector3 Pivot)
    {
        int i = Random.Range(0, tiles.Count);
        
        Vector3 Direction = Vector3.Normalize(Pivot - Position);

        GameObject nextTile = Instantiate(tiles[i], Position, Quaternion.identity);
        nextTile.transform.forward = Direction;


        
    }
}
