using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;

	[SerializeField]
	private Camera cam;
	private const float speed = 1f;
	private const float lookSensitivity = 1f;
	private const float viewRange = 65.0f;
	private const float deathTime = 10.0f;

	[SerializeField]
	private const float deathRadius = 10f;
	private float deathTimer = 0;

	public float playerFuel = 0f;

	private float rotY = 0;
	private float rotX = 0;

	public AudioSource ignitor;
	public AudioSource extinguisher;
	public AudioSource flame;
	public AudioSource ignition;

	private bool lanternOn = false;
	private int ambiantOnly = 0;
	private int remainingLightAttempts = 100;

	public Light mainLight;
	public Light ambiantLight;

	private GameObject cart;

    // Start is called before the first frame update
    void Start()
    {

    	Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();

        cart = GameObject.Find("Cart");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		//Walking Audio
		if (Input.GetKeyDown(KeyCode.W))
		{
			GetComponent<AudioSource>().Play();
		}
		else if (Input.GetKeyUp(KeyCode.W))
		{
			GetComponent<AudioSource>().Stop();
		}

		//Calculate movement velocity as a 3D vector
		float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        if(velocity != Vector3.zero)
        {
        	rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        //Calculate rotation as a 3D vector and apply
        rotX += Input.GetAxis("Mouse X") * lookSensitivity;

        //This is necessary
        transform.rotation = Quaternion.Euler(0f, rotX, 0f);

		rotY += Input.GetAxis("Mouse Y") * lookSensitivity;
  		rotY = Mathf.Clamp(rotY, -viewRange, viewRange);
   		cam.transform.rotation = Quaternion.Euler(-rotY, rotX, 0f);


		//================================

		//Lantern
		if (Input.GetButtonDown("Fire2"))
		{
			if (lanternOn)
			{
				flame.Stop();
				extinguisher.Play();
				lanternOn = false;
				remainingLightAttempts = Random.Range(0, 5);
			}
			else
			{
				if (remainingLightAttempts == 0)
				{
					lanternOn = true;
					ignitor.Play();
					ignition.Play();
					flame.Play();
				}
				else
				{
					remainingLightAttempts -= 1;
					ignitor.Play();
					ambiantOnly = Random.Range(4, 9);
				}
			}

		}

		//Enabling the lights when lanternOn is true
		mainLight.enabled = lanternOn;
		ambiantLight.enabled = lanternOn;

		//allows the ambient light to be on by itself for ambientOnly number of frames (used for sparking)
		if (!lanternOn && ambiantOnly > 0)
		{
			ambiantLight.enabled = true;
			ambiantOnly -= 1;
		}
		else if (lanternOn && ambiantOnly > 0)
		{
			ambiantOnly = 0;
		}

		//Look and Interact
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;

			Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);

			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1000))
			{
				GameObject hitObject = hit.collider.GetComponentInParent<Transform>().gameObject;

				if (hitObject.tag == "FuelCell")
				{

					FuelCellController cell = hitObject.GetComponent<FuelCellController>();

					playerFuel += cell.fuelAmount;
			
					cell.Consume();
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

				//Visual indicator
				//audio indicator
			}
		}

		//================================

		if(!lanternOn && Vector3.Distance(cart.transform.position, transform.position) > deathRadius)
		{
			//Death timer
			if(deathTimer > deathTime)
			{
				//Die

				//Play death sound

				//Go back to main menu scene
				SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
			}
			else
			{
				//Progress dying
				deathTimer += Time.deltaTime;
			}
		}
	}
}
