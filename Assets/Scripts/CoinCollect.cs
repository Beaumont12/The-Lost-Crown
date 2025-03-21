using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    public AudioClip collectSound; // optional

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound if available
            if (collectSound)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Update coins in UI
            GameUI gameUI = FindObjectOfType<GameUI>();
            if (gameUI != null)
            {
                gameUI.AddCoin();
            }

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}
