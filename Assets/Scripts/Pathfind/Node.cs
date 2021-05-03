namespace Pathfind
{
    public class Node
    {
        private bool _walkable;
        private  int _gridX;
        private  int _gridY;
        private  float _penalty;

        private  int _gCost;
        private  int _hCost;
        private  Node _parent;

        public Node(float _price, int _gridX, int _gridY)
        {
            _walkable = _price != 0.0f;
            _penalty = _price;
            this._gridX = _gridX;
            this._gridY = _gridY;
        }

        public int fCost
        {
            get
            {
                return _gCost + _hCost;
            }
        }

        public bool Walkable { get => _walkable; set => _walkable = value; }
        public int GridX { get => _gridX; set => _gridX = value; }
        public int GridY { get => _gridY; set => _gridY = value; }
        public float Penalty { get => _penalty; set => _penalty = value; }
        public int GCost { get => _gCost; set => _gCost = value; }
        public int HCost { get => _hCost; set => _hCost = value; }
        public Node Parent { get => _parent; set => _parent = value; }
    }
}