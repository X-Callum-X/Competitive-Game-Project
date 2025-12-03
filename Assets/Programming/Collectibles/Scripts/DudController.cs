using Unity.VisualScripting;
using UnityEngine;

public class DudController : MonoBehaviour
{
    private PlayerController player;

    public ParticleSystem collectEffect;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private DudSO collectible;

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

            player.playerCurrency += collectible.pointValue;
            Destroy(this.gameObject);
        }
    }
}
