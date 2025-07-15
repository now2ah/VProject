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
                    return new NormalBlock(index, Color.clear);

                case EBlockType.Normal:
                    Random random = new Random();
                    int randomColor = random.Next(0, Enum.GetValues(typeof(ENormalBlockColor)).Length);
                    switch ((ENormalBlockColor)randomColor)
                    {
                        case ENormalBlockColor.Red:
                            return new NormalBlock(index, Color.red);
                        case ENormalBlockColor.Yellow:
                            return new NormalBlock(index, Color.yellow);
                        case ENormalBlockColor.Green:
                            return new NormalBlock(index, Color.green);
                        default:
                            throw new Exception("Invalid Normal Block Color");
                    }
                default:
                    throw new Exception("Invalid Block Type");
            }
        }
    }
}
