using System.IO;
using UnityEngine;

public class SawbladeController : MonoBehaviour
{
    private PlayerController player;

    public Vector3 pointA;
    public Vector3 pointB;

    public float moveSpeed = 5;

    bool toggleMovement = false;

    float lerpValue = 0;

    public int rotationSpeed;

    private float damageCooldown;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }
    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    void FixedUpdate()
    {
        if (toggleMovement)
        {
            lerpValue += moveSpeed * Time.deltaTime;

            if (lerpValue >= 1f)
            {
                lerpValue = 1f;
                toggleMovement = false;
            }
        }
        else
        {
            lerpValue -= Time.fixedDeltaTime * moveSpeed;

            if (lerpValue <= 0f)
            {
                lerpValue = 0f;
                toggleMovement = true;
            }
        }

        transform.position = Vector3.Lerp(pointA, pointB, lerpValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player.TakeDamage(1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            damageCooldown += Time.deltaTime;

            if (damageCooldown >= 1f)
            {
                damageCooldown = 0;
                player.TakeDamage(1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            damageCooldown = 0;
        }
    }
}
