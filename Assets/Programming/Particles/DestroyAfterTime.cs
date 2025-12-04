using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    private void Update()
    {
        if (gameObject.activeSelf)
        {
            Destroy(gameObject, 2);
        }
    }
}
