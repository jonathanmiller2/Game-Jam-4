using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerMenuController : MonoBehaviour
{

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private float speed = 5f;

	[SerializeField]
	private float lookSensitivity = 1f;

	private GameObject grabbed = null;

	float doorActionDelay = 1.3f;
	float actionTime = 0f;
	bool doorActionPlay = false;

	// Start is called before the first frame update
	void Start()
	{

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

	}

	// Update is called once per frame
	void Update()
	{

		if (actionTime > 0f)
		{
			if (Time.time > actionTime + doorActionDelay)
			{
				if (doorActionPlay)
				{
					SceneManager.LoadScene(1, LoadSceneMode.Single);
				}
				else
				{
					Application.Quit();
				}
			}
		}

		UndoAllColor();

		float yRot = Input.GetAxisRaw("Mouse X");
		//float xRot = Input.GetAxisRaw("Mouse Y");

		Vector3 cameraTilt = new Vector3(-0f, yRot, 0f) * lookSensitivity;
		cam.transform.Rotate(cameraTilt);

		float camX = cam.transform.rotation.eulerAngles.x;
		float camY = cam.transform.rotation.eulerAngles.y;

		if (camX > 180f)
		{
			cam.transform.rotation = Quaternion.Euler(camX, 180f, 0f);
			camX = 180f;
		}
		else if (camX < 0f)
		{
			cam.transform.rotation = Quaternion.Euler(camX, 0f, 0f);
			camX = 0f;
		}

		if (Input.GetButtonUp("Fire1"))
		{
			grabbed = null;
		}

		if (Input.GetButton("Fire1"))
		{
			if (grabbed)
			{
				RaycastHit hit;

				if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000))
				{
					GameObject hitObject = hit.collider.GetComponentInParent<Transform>().gameObject;

					if (hitObject == grabbed)
					{
						float newZ = hit.point.z;

						if (newZ > 2f)
						{
							newZ = 2f;
						}
						else if (newZ < -2f)
						{
							newZ = -2f;
						}

						grabbed.transform.position = new Vector3(grabbed.transform.position.x, grabbed.transform.position.y, newZ);
						SetVolumeFromZ(newZ);
					}

				}
			}
		}

		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;

			Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);

			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000))
			{
				GameObject hitObject = hit.collider.GetComponentInParent<Transform>().gameObject;

				if (hitObject.tag == "PlayButton")
				{
					hitObject.GetComponent<AudioSource>().Play();
					doorActionPlay = true;
					actionTime = Time.time;
					
				}
				else if (hitObject.tag == "ExitButton")
				{
					hitObject.GetComponent<AudioSource>().Play();
					actionTime = Time.time;
				}
				else if (hitObject.tag == "Slider")
				{
					grabbed = hitObject;

					grabbed.transform.position = new Vector3(grabbed.transform.position.x, grabbed.transform.position.y, hit.point.z);
					SetVolumeFromZ(hit.point.z);
				}
				else
				{
					grabbed = null;
				}
			}
		}
		else
		{
			RaycastHit hit;

			Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);

			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000))
			{
				GameObject hitObject = hit.collider.GetComponentInParent<Transform>().gameObject;

				if (hitObject.tag == "PlayButton" || hitObject.tag == "ExitButton" || hitObject.tag == "Slider")
				{
					hitObject.GetComponent<Renderer>().material.color = new Color(0, 0, 0);
					//Do actual indication stuff here @camden
				}
			}
		}

		void UndoAllColor()
		{
			GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Renderer>().material.color = new Color(255, 0, 0);
			GameObject.FindGameObjectWithTag("ExitButton").GetComponent<Renderer>().material.color = new Color(255, 0, 0);
			GameObject.FindGameObjectWithTag("Slider").GetComponent<Renderer>().material.color = new Color(255, 0, 0);
		}

		void SetVolumeFromZ(float Z)
		{
			float value = 1 - (Math.Abs(-2f - Z) / 4f);

			AudioListener.volume = value;

		}

	}
}