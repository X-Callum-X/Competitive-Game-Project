using UnityEngine;

public class SecretCollectibleController : MonoBehaviour
{
    private PlayerController player;

    public ParticleSystem collectEffect;

    public int rotationSpeed;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);

            player.secretsCollected += 1;
            Destroy(this.gameObject);
        }
    }
}
