using System;
using UnityEngine;
using VProject.Domains;
using Random = System.Random;

namespace VProject.Utils
{
    public class BlockFactory
    {
        public Block CreateBlock(Vector2Int index, EBlockType type)
        {
            switch (type)
            {
                case EBlockType.Red:
                    return new Block(index, EBlockType.Red);
                case EBlockType.Yellow:
                    return new Block(index, EBlockType.Yellow);
                case EBlockType.Green:
                    return new Block(index, EBlockType.Green);
                default:
                    return new Block(index, EBlockType.None);
            }
        }

        public Block CreateRandomBlock(Vector2Int index)
        {
            Random random = new Random();
            int randomType = random.Next(1, Enum.GetValues(typeof(EBlockType)).Length);
            return new Block(index, (EBlockType)randomType);
        }
    }
}
