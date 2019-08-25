using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCellController : MonoBehaviour
{
	public float fuelAmount = 0.3f;
	private float timer = 0.8f;
	private float startTime = -1f;

	private void Update()
	{
		if (startTime > 0)
		{
			if (Time.time >= timer + startTime)
			{
				Destroy(gameObject);
			}
		}
	}

	public void Consume()
	{
		if (!GetComponent<AudioSource>().isPlaying)
		{
			timer = GetComponent<AudioSource>().clip.length;
			GetComponent<AudioSource>().Play();
			startTime = Time.time;
		}
	}
}