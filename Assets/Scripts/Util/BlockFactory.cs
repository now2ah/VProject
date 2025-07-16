using System;
using UnityEngine;
using VProject.Domains;
using static VProject.Domains.NormalBlock;
using Random = System.Random;

namespace VProject.Utils
{
    public class BlockFactory
    {
        public Block CreateBlock(Vector2Int index, EBlockType type)
        {
            switch (type)
            {
                case EBlockType.None:
                    Block block = new NormalBlock(index, Color.clear, new NormalBlockEffect());
                    block.Break();
                    return block;

                case EBlockType.Normal:
                    Random random = new Random();
                    int randomColor = random.Next(0, Enum.GetValues(typeof(ENormalBlockColor)).Length);
                    switch ((ENormalBlockColor)randomColor)
                    {
                        case ENormalBlockColor.Red:
                            return new NormalBlock(index, Color.red, new NormalBlockEffect());
                        case ENormalBlockColor.Yellow:
                            return new NormalBlock(index, Color.yellow, new NormalBlockEffect());
                        case ENormalBlockColor.Green:
                            return new NormalBlock(index, Color.green, new NormalBlockEffect());
                        default:
                            throw new Exception("Invalid Normal Block Color");
                    }
                default:
                    throw new Exception("Invalid Block Type");
            }
        }
    }
}
