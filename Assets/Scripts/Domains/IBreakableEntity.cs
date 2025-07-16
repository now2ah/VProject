using System;
using UnityEngine;
using VProject.Services;

namespace VProject.Domains
{
    public interface IBreakableEntity
    {
        public EBlockType GetBlockType();
        public Vector2Int GetIndex();
        public BreakResult Break();
        public void ApplyEffect(GridService gridService);
        public bool IsEmptyBlock();
        public void ChangeIndex(Vector2Int destinationIndex);

        public event Action<Vector2Int> OnIndexChanged;
    }
}
