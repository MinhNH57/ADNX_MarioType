using Unity.Netcode;
using UnityEngine;

public class BrickHitEffect : NetworkBehaviour
{
    public GameObject hitParticlePrefab;
    public AudioManager _audioManager;

    private void Awake()
    {
        _audioManager =
            GameObject.FindGameObjectWithTag("Audio")
            .GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                HitBrickServerRpc(contact.point);
                break;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void HitBrickServerRpc(Vector2 pos)
    {
        TriggerEffectClientRpc(pos);

        GameManager.Instance.AddCoin(10);

        GetComponent<NetworkObject>().Despawn();
    }

    [ClientRpc]
    void TriggerEffectClientRpc(Vector2 pos)
    {
        GameObject effect =
            Instantiate(
                hitParticlePrefab,
                pos,
                Quaternion.identity
            );

        effect.transform.localScale =
            new Vector3(0.5f, 0.5f, 1f);

        ParticleSystem ps =
            effect.GetComponent<ParticleSystem>();

        if (ps != null)
            ps.Play();

        if (_audioManager != null)
        {
            _audioManager.PlaySfx(
                _audioManager.breakClip
            );
        }

        Destroy(effect, 2f);
    }
}