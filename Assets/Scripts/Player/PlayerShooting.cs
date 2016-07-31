using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;


    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
	float camRayLength = 100f;
	Vector3 hitPoint;
	float lastShot;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }

	/* Shooting mechanics:
	 * Left click on enemy = player stops movement, turns towards enemy, and shoots
	 * Right click = player stops movement, turns to click position, and shoots
	 */
    void Update ()
    {
        timer += Time.deltaTime;

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;
		int floormask = (1 << 8) | (1 << 9);

		// Left click on enemy
		if (Input.GetMouseButtonDown (0)) 
		{
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floormask)) 
			{
				if (floorHit.transform.gameObject.layer == 9) 
				{
					Shoot ();
				}
			}
		} 

		// Right click
		else if (Input.GetMouseButton (1)) 
		{
			if (Physics.Raycast (camRay, out floorHit, camRayLength, floormask)) 
			{
				if(Time.time - lastShot > 0.5f)
					Shoot ();
			}
		}

        if(timer >= timeBetweenBullets * effectsDisplayTime)
            DisableEffects ();
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
		lastShot = Time.time;
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
