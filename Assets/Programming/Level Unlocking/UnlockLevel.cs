using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockLevel : MonoBehaviour
{
    public PlayerController player;

    public int secretLevel;

    private void Start()
    {
        secretLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void UnlockSecretLevel()
    {
        if (secretLevel > PlayerPrefs.GetInt("levelAt") && player.secretsCollected >= 3)
        {
            PlayerPrefs.SetInt("levelAt", secretLevel);
        }
    }
}
