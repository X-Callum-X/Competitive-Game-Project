using UnityEngine;

public class CheckIfPlayerOnMovingPlatform : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            player.transform.localScale = Vector3.one;
            other.transform.SetParent(gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
