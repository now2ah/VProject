
using System;
using UnityEngine;

namespace VProject.Domains
{
    public enum EBlockType
    {
        None,
        Red,
        Yellow,
        Green,
    }

    public class Block
    {
        private Vector2Int _index;
        [SerializeField] private EBlockType _type;

        public Vector2Int Index => _index;
        public EBlockType Type => _type; 

        public event Action<Vector2Int> OnValueChanged;

        public Block(Vector2Int index, EBlockType type)
        {
            _index = index;
            _type = type;
        }

        public void ChangeIndex(Vector2Int index)
        {
            _index = index;
            OnValueChanged?.Invoke(index);
        }

        public void Destroy()
        {
            Debug.Log($"{_index} , {_type}block destroyed");
            _type = EBlockType.None;
        }

        public bool IsEmptyBlock()
        {
            return _type == EBlockType.None;
        }
    }
}
