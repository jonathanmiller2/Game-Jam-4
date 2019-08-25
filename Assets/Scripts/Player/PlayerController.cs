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

	private bool lanternOn = true;

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
