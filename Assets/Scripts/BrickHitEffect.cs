using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BrickHitEffect : MonoBehaviour
{
    public GameObject hitParticlePrefab;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                Debug.Log("Da cham roi nhe");
                TriggerEffect(contact.point);
                break;
            }
        }
    }

    void TriggerEffect(Vector2 pos)
    {
        GameObject effect = Instantiate(hitParticlePrefab, pos, Quaternion.identity);
        effect.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Play();
        gameObject.SetActive(false);
        GameManager.Instance.AddCoin(10);
        Destroy(effect, 2f);
    }
}