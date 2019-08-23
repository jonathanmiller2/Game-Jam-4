using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawning : MonoBehaviour
{
	[SerializeField]
	private float timeBetweenSpawnChecks = .5f;
    private float spawnCheckTimer = 0f;

    private GameObject player;
    private GameObject Contro
    private List<GameObject> snapPoints;

    [SerializeField]
    private float SpawnDistance = 10f;

    void Start()
    {
    	player = GameObject.Find("Player")
		snapPoints = GetChildObjectsWithTag("SnapPoint");

    	
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnCheckTimer > timeBetweenSpawnChecks)
        {
        	spawnCheckTimer = 0f;

        	for(GameObject snap : snapPoints)
        	{
        		if(Vector3.Distance(snap.transform.position, player.transform.position) < SpawnDistance)
        		{

        		}
        	}
        	
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
}
