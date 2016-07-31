using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    float camRayLength = 100f;
	Vector3 hitPoint;

    void Awake() 
    {
        anim = GetComponent<Animator> ();
        playerRigidbody = GetComponent<Rigidbody> ();
		hitPoint = transform.position;
    }
    
	/* Movement mechanics:
	 * Player movement is controlled through point-and-click implementation.
	 * Left click on floor = player moves to point
	 * Left click on enemy = player stops movement, turns towards enemy and shoots
	 * Right click = player stops movement, turns towards click posiiton, and shoots, regardless of whether or not an enemy is in that position
	 */
    void Update() 
    {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;
		int floormask = (1 << 8) | (1 << 9);

		// Left click
		if (Input.GetMouseButton (0)) 
		{
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floormask)) 
			{
				hitPoint = new Vector3 (floorHit.point.x, 0.0f, floorHit.point.z);
				Turn (hitPoint);

				// Left click on enemy
				if (floorHit.transform.gameObject.layer == 9) 
				{
					hitPoint = new Vector3 (floorHit.collider.gameObject.transform.position.x, 0.0f, floorHit.collider.gameObject.transform.position.z);
					Turn (hitPoint);

					hitPoint = new Vector3 (transform.position.x, 0.0f, transform.position.z);
				}
			}
		}

		// Right click
		else if (Input.GetMouseButton (1)) 
		{
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floormask)) 
			{
				hitPoint = new Vector3 (floorHit.point.x, 0.0f, floorHit.point.z);
				Turn (hitPoint);

				hitPoint = new Vector3 (transform.position.x, 0.0f, transform.position.z);
			}
		}
    }

	void FixedUpdate()
	{
		Move (hitPoint);
	}
    
	void Move(Vector3 moveTo)
    {
		transform.position = Vector3.MoveTowards (transform.position, moveTo, 0.075f);
		if(moveTo != transform.position)
			anim.SetBool ("IsWalking", true);
		else
			anim.SetBool ("IsWalking", false);
    }

	void Turn(Vector3 lookAt)
	{
		Vector3 playerToMouse = hitPoint - transform.position;
		playerToMouse.y = 0f;

		Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
		playerRigidbody.MoveRotation (newRotation);
	}
}