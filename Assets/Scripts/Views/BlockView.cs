using UnityEngine;
using VProject.Domains;

namespace VProject.Views
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        public void SetColor(EBlockType type)
        {
            switch (type)
            {
                case EBlockType.Red:
                    _renderer.material.color = Color.red;
                    break;
                case EBlockType.Yellow:
                    _renderer.material.color = Color.yellow;
                    break;
                case EBlockType.Green:
                    _renderer.material.color = Color.green;
                    break;
            }
        }
    }
}
