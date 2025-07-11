using UnityEngine;
using VProject.Services;

namespace VProject.Views
{
    public class Background : MonoBehaviour
    {
        private const float BACKGROUND_Z = 1f;

        [SerializeField] private GridController _gridController;

        private void OnEnable()
        {
            _gridController.OnGridViewGenerated += GridController_OnGridViewGenerated;
        }

        private void OnDisable()
        {
            _gridController.OnGridViewGenerated -= GridController_OnGridViewGenerated;
        }

        private void GridController_OnGridViewGenerated(Vector3 centerPosition, int gridSize)
        {
            transform.position = new Vector3(centerPosition.x, centerPosition.y, BACKGROUND_Z);

            float scaleRatio = gridSize / 4;
            if (scaleRatio < 1)
            {
                scaleRatio = 1;
            }

            transform.localScale = new Vector3(scaleRatio, 1f, scaleRatio);
            if (transform.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.material.mainTextureScale = new Vector2(scaleRatio, scaleRatio);
            }
        }
    }
}