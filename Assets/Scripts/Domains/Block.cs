
using System;
using UnityEngine;
using VProject.Services;

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
        ColorBomb
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

        public virtual void ApplyEffect(GridService gridService)
        {
            //do nothing
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

    public class NormalBlock : Block, IColorBlock
    {
        public enum ENormalBlockColor
        {
            Red,
            Yellow,
            Green
        }

        private Color _blockColor;
        private IBlockEffect _blockEffect;

        public Color GetBlockColor() => _blockColor;

        public NormalBlock(Vector2Int index, Color blockColor, IBlockEffect blockEffect) : base(index)
        {
            _index = index;
            _type = EBlockType.Normal;
            _state = EBlockState.Idle;
            _blockColor = blockColor;
            _blockEffect = blockEffect;
        }

        public override BreakResult Break()
        {
            BreakResult result = base.Break();
            result.rewardScore = 10;

            return result;
        }

        public override void ApplyEffect(GridService gridService)
        {
            _blockEffect.Execute(_index, gridService);
        }
    }

    public class ColorBombBlock : Block, IColorBlock
    {
        private Color _blockColor;
        private IBlockEffect _blockEffect;

        public Color GetBlockColor() => _blockColor;

        public ColorBombBlock(Vector2Int index, Color blockColor, IBlockEffect blockEffect) : base(index)
        {
            _index = index;
            _type = EBlockType.ColorBomb;
            _state = EBlockState.Idle;
            _blockColor = blockColor;
            _blockEffect = blockEffect;
        }

        public override BreakResult Break()
        {
            BreakResult result = base.Break();
            result.rewardScore = 10;

            return result;
        }

        public override void ApplyEffect(GridService gridService)
        {
            _blockEffect.Execute(_index, gridService);
        }
    }
}
