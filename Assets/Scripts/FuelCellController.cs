using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCellController : MonoBehaviour
{
	public float fuelAmount = 4f;


	public void Consume()
	{
		//play sound
		//wait
		Destroy(gameObject);
	}
}