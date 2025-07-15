
using System;
using UnityEngine;

namespace VProject.Domains
{
    public struct BreakResult
    {
        public Vector2Int index;
        public int rewardScore;

        public BreakResult(Vector2Int index, int rewardScore)
        {
            this.index = index;
            this.rewardScore = rewardScore;
        }
    }

    public enum EBlockType
    {
        None,
        Normal,
    }

    public enum EBlockState
    {
        Empty,
        Idle,
    }

    public abstract class Block : IBreakableEntity
    {
        protected Vector2Int _index;
        [SerializeField] protected EBlockType _type;
        protected EBlockState _state;

        public Vector2Int GetIndex() => _index;
        public EBlockType GetBlockType() => _type;
        public EBlockState GetBlockState() => _state;

        public event Action<Vector2Int> OnIndexChanged;

        public Block(Vector2Int index)
        {
            _index = index;
            _type = EBlockType.None;
            _state = EBlockState.Empty;
        }

        public virtual BreakResult Break()
        {
            _state = EBlockState.Empty;
            return new BreakResult(_index, 0);
        }

        public bool IsEmptyBlock()
        {
            return _state == EBlockState.Empty;
        }

        public void ChangeIndex(Vector2Int index)
        {
            _index = index;
            OnIndexChanged?.Invoke(index);
        }

        public void SetStateTest(EBlockState state)
        {
            _state = state;
        }
    }

    public class NormalBlock : Block
    {
        public enum ENormalBlockColor
        {
            Red,
            Yellow,
            Green
        }

        private Color _blockColor;

        public Color BlockColor => _blockColor;

        public NormalBlock(Vector2Int index, Color blockColor) : base(index)
        {
            _index = index;
            _type = EBlockType.Normal;
            _state = EBlockState.Idle;
            _blockColor = blockColor;
        }

        public override BreakResult Break()
        {
            BreakResult result = base.Break();
            result.rewardScore = 10;
            return result;
        }
    }
}
