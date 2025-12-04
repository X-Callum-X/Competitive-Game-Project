using UnityEngine;

public class EndCollectible : MonoBehaviour
{
    public GameObject youWinScreen;

    public int rotationSpeed;

    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0;
        Destroy(this.gameObject);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        youWinScreen.SetActive(true);
    }
}
