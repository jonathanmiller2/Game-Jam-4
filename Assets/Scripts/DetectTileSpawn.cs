using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTileSpawn : MonoBehaviour
{
	[SerializeField]
	private float timeBetweenSpawnChecks = 1f;

    [SerializeField]
    private float spawnCheckTimer = 0f;

    private GameObject player;
    private GameObject tileManager;
    private TileSpawn tileSpawn;
    private List<GameObject> snapPoints;
    private GameObject pivotPoint;

    [SerializeField]
    private float SpawnDistance = 1f;

    void Start()
    {
    	player = GameObject.Find("Player");
    	tileManager = GameObject.Find("TileManager");
        tileSpawn = tileManager.GetComponent<TileSpawn>();
		snapPoints = GetChildObjectsWithTag("SnapPoint");
        pivotPoint = GetChildObjectWithTag(transform, "PivotPoint");
    	
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCheckTimer > timeBetweenSpawnChecks)
        {
        	spawnCheckTimer = 0f;

            //Create copy of snapPoints, so we can edit snapPoints inside of the loop that iterates over it
            List<GameObject> unusedSnapPoints = new List<GameObject>();

        	foreach(GameObject snap in snapPoints)
        	{
                
        		if(Vector3.Distance(snap.transform.position, player.transform.position) < SpawnDistance)
        		{
        			tileSpawn.Spawn(snap.transform.position, pivotPoint.transform.position);
        		}
                else
                {
                    //Store all of the unused snap points
                    unusedSnapPoints.Add(snap);
                }
        	}

            //Keep track of only the unused snap points
            snapPoints = unusedSnapPoints;
        }
        else
        {
        	spawnCheckTimer += Time.deltaTime;
        }
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
