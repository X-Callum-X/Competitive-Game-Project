using UnityEngine;
using UnityEngine.Rendering;

public class SpawnDuds : MonoBehaviour
{
    [SerializeField] GameObject dud;

    [SerializeField] int numOfDuds = 5;

    [SerializeField] float radius = 5f;

    public void DropDuds()
    {
        for (int i = 0; i < numOfDuds; i++)
        {
            float angle = i * Mathf.PI * 2 / numOfDuds;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
            Instantiate(dud, pos, rot);
        }
    }
}
