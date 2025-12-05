using UnityEngine;

public class DeletePlayerPrefs : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
