using System;
using UnityEngine;
using VProject.Domains;
using static VProject.Domains.NormalBlock;
using Random = System.Random;

namespace VProject.Utils
{
    public class BlockFactory
    {
        private readonly Random _random = new Random();

        public Block CreateBlock(Vector2Int index, EBlockType type)
        {
            switch (type)
            {
                case EBlockType.None:
                    Block block = new NormalBlock(index, Color.clear, new NormalBlockEffect());
                    block.Break();
                    return block;

                case EBlockType.Normal:
                    int randomColorNormal = _random.Next(0, Enum.GetValues(typeof(ENormalBlockColor)).Length);
                    switch ((ENormalBlockColor)randomColorNormal)
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

                case EBlockType.ColorBomb:
                    int randomColorCB = _random.Next(0, Enum.GetValues(typeof(ENormalBlockColor)).Length);
                    switch ((ENormalBlockColor)randomColorCB)
                    {
                        case ENormalBlockColor.Red:
                            return new ColorBombBlock(index, Color.red, new ColorBombEffect());
                        case ENormalBlockColor.Yellow:
                            return new ColorBombBlock(index, Color.yellow, new ColorBombEffect());
                        case ENormalBlockColor.Green:
                            return new ColorBombBlock(index, Color.green, new ColorBombEffect());
                        default:
                            throw new Exception("Invalid Color Bomb Block Color");
                    }
                default:
                    throw new Exception("Invalid Block Type");
            }
        }
    }
}
