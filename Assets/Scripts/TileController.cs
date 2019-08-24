using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
	[SerializeField]
	private float timeBetweenSpawnChecks = 1f;

    [SerializeField]
    private float spawnCheckTimer = 0f;

    [SerializeField]
    private float SpawnDistance = 1f;

    [SerializeField]
    private float DeleteDistance = 10000f;    

    private GameObject player;
    private GameObject tileManager;
    private TileSpawn tileSpawn;
    private List<GameObject> snapPoints;
    private GameObject centerPoint;

    private GameObject childTile;
    private GameObject parentTile;


    void Awake()
    {
    	player = GameObject.Find("Player");
    	tileManager = GameObject.Find("TileManager");
        tileSpawn = tileManager.GetComponent<TileSpawn>();
		snapPoints = GetChildObjectsWithTag("SnapPoint");
        centerPoint = GetChildObjectWithTag(transform, "CenterPoint");
    }

    // Update is called once per frame
    public List<GameObject> SpawnOffOfThisTile()
    {
        //Create copy of snapPoints, so we can edit snapPoints inside of the loop that iterates over it
        List<GameObject> newPlatforms = new List<GameObject>();

        foreach(GameObject snap in snapPoints)
        {
            GameObject newPlatform = tileSpawn.Spawn(snap.transform.position, centerPoint.transform.position);
        	newPlatforms.Add(newPlatform);
        }

        return newPlatforms;
    }

    public List<GameObject> GetChildObjectsWithTag(string Tag)
    {
    	List<GameObject> res = new List<GameObject>();

       	for(int i = 0; i < transform.childCount; i++)
       	{
          	Transform child = transform.GetChild(i);

           	if(child.tag == Tag)
           	{
               	res.Add(child.gameObject);
           	}
       	}
       	return res;
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
