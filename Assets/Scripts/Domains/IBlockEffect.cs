using UnityEngine;
using VProject.Services;

namespace VProject.Domains
{
    public interface IBlockEffect
    {
        public void Execute(Vector2Int index, GridService gridService);
    }
}
