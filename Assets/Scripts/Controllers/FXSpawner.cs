using System.Collections;
using UnityEngine;

namespace VProject.Controllers
{
    public class FXSpawner : MonoBehaviour
    {
        public enum EFx
        {
            HIT,
            COLORBOMB
        }

        [SerializeField] private Transform _hitPrefab;
        [SerializeField] private Transform _colorBombHitPrefab;

        public void SpawnFX(Vector3 spawnPosition, EFx fxType)
        {
            Transform fxTransform = null;
            switch (fxType)
            {
                case EFx.HIT:
                    fxTransform = Instantiate(_hitPrefab, spawnPosition, Quaternion.identity);
                    StartCoroutine(DestroyFX(fxTransform));
                    break;
                case EFx.COLORBOMB:
                    fxTransform = Instantiate(_colorBombHitPrefab, spawnPosition, Quaternion.identity);
                    StartCoroutine(DestroyFX(fxTransform));
                    break;
            }
        }

        private IEnumerator DestroyFX(Transform fxTransform)
        {
            if (fxTransform.TryGetComponent<ParticleSystem>(out ParticleSystem particle))
            {
                yield return new WaitUntil(() => particle.isPlaying == false);
            }

            yield return null;
            Destroy(fxTransform.gameObject);
        }
    }
}

