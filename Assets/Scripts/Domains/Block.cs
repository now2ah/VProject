
namespace VProject.Domains
{
    public enum EBlockType
    {
        None,
        Red,
        Yellow,
        Green,
    }

    public struct Block
    {
        public EBlockType type;

        public Block(EBlockType type)
        {
            this.type = type;
        }

        public void Destroy()
        {
            type = EBlockType.None;
        }
    }
}
