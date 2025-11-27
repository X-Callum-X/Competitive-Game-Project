using Unity.VisualScripting;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private PlayerController player;

    public ParticleSystem collectEffect;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private CollectibleSO collectible;

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
            Debug.Log(player.playerCurrency);
            Destroy(this.gameObject);
        }
    }
}
