
using System;
using VProject.Domains;

namespace VProject.Util
{
    public class BlockFactory
    {
        public Block Create(EBlockType type)
        {
            switch (type)
            {
                case EBlockType.Red:
                    return new Block(EBlockType.Red);
                case EBlockType.Yellow:
                    return new Block(EBlockType.Yellow);
                case EBlockType.Green:
                    return new Block(EBlockType.Green);
                default:
                    return new Block(EBlockType.None);
            }
        }

        public Block CreateRandomBlock()
        {
            Random random = new Random();
            int randomType = random.Next(1, Enum.GetValues(typeof(EBlockType)).Length);
            return new Block((EBlockType)randomType);
        }
    }
}
