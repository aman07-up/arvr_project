using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float bulletLifetime = 5f; // seconds before bullet is destroyed

    [Header("Sound Settings")]
    public AudioClip clip;
    private AudioSource source;

    [Header("UI Control")]
    public Button fireButton; // assign in inspector (from Canvas)

    private void Start()
    {
        source = GetComponent<AudioSource>();

        // 🔹 Link FireButton to FireBullet method if assigned
        if (fireButton != null)
        {
            fireButton.onClick.AddListener(FireBullet);
        }
    }

    public void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 🔹 Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 🔹 Add velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        // 🔹 Play sound
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip);
        }

        // 🔹 Destroy after lifetime
        Destroy(bullet, bulletLifetime);
    }
}
