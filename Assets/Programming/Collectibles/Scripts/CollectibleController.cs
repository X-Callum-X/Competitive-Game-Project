using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] private CollectibleSO collectible;
    private PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.playerCurrency += collectible.pointValue;
            Destroy(this.gameObject);
        }
    }
}
