using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightTargetController : MonoBehaviour
{

	private Vector3 start = new Vector3(5f, -10f, 3f);
	private Vector3 end = new Vector3(-5f, -10f, 3f);

	private bool tog = true;

    // Update is called once per frame
    void Update()
    {
		transform.position = Vector3.Lerp(start, end, (Mathf.Sin(Time.time) + 1) / 2);

		//if ((Mathf.Sin(Time.time) + 1) / 2 > 1)
		//{
		//	Debug.Log((Mathf.Sin(Time.time) + 1) / 2);
		//}

		//if (tog)
		//{
		//	transform.position = Vector3.Lerp(start, end, (Mathf.Sin(Time.time) + 1) / 2);
		//}
		//else
		//{
		//	transform.position = Vector3.Lerp(end, start, (Mathf.Sin(Time.time) + 1) / 2);
		//}

		//if (tog && Vector3.Distance(transform.position, end) < 0.5f)
		//{
		//	tog = false;
		//}
		//else if (!tog && Vector3.Distance(transform.position, start) < 0.5f)
		//{
		//	tog = true;
		//}
		
	}
}
