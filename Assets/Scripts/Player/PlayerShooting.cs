using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public int magSize = 30;
    public float reloadDelay = 2.5f;
    public float timeBetweenBullets = 0.15f;        
    public float range = 100f;
    public Text ammoText;
    public Slider reloadSlider;

    int shootableMask;
    int currentMag;
    float timer;
    float reloadTimer;
    float effectsDisplayTime = 0.2f;


    Ray shootRay = new Ray();
    RaycastHit shootHit;

    bool isReloading;
    
    ParticleSystem gunParticles;                    
    LineRenderer gunLine;                           
    AudioSource gunAudio;                           
    Light gunLight;                                 
               

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
        currentMag = magSize;
    }


    void Update()
    {
        timer += Time.deltaTime;

        // mengecek jika player sedang reload, maka akan melakukan sequence reload

        if (isReloading)
        {
            reloadSlider.gameObject.SetActive(true);
            reloadTimer += Time.deltaTime;
            reloadSlider.value = (reloadTimer / reloadDelay) * 100;            


            if (reloadTimer >= reloadDelay)
            {
                isReloading = false;
                currentMag = magSize;
                reloadTimer = 0;
                reloadSlider.gameObject.SetActive(false);
            }
        }

        if (currentMag > 0 && Input.GetButton("Fire1") && timer >= timeBetweenBullets && !isReloading)
        {
            Shoot();
        }

        if (currentMag != magSize && Input.GetKey(KeyCode.R))
        {
            Reload();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

        ammoText.text = string.Format("Ammo: {0}", currentMag);
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void Shoot()
    {
        timer = 0f;
        currentMag--;
        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    private void Reload()
    {
        isReloading = true;
    }

}