using Unity.VisualScripting;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private PlayerController player;

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
            player.playerCurrency += collectible.pointValue;
            Debug.Log(player.playerCurrency);
            Destroy(this.gameObject);
        }
    }
}
