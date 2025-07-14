using System.Collections;
using UnityEngine;

namespace VProject.Controllers
{
    public class FXSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _hitPrefab;

        public void SpawnFX(Vector3 spawnPosition)
        {
            Transform fxTransform = Instantiate(_hitPrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(DestroyFX(fxTransform));
        }

        private IEnumerator DestroyFX(Transform fxTransform)
        {
            yield return new WaitForSeconds(3f);
            Destroy(fxTransform.gameObject);
        }
    }
}

