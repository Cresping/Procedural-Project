namespace HeroesGames.ProjectProcedural.Pathfind
{
    /// <summary>
    /// Clase encargada de almaacenar la información de un nodo. 
    /// </summary>
    public class Node
    {
        private int _gridX;
        private int _gridY;
        //Coste de caminar por el nodo
        private float _penalty;
        //Si se puede caminar por el o no
        private bool _walkable;

        private  int _gCost;
        private  int _hCost;
        private  Node _parent;

        /// <summary>
        /// Constructor parametrizado
        /// </summary>
        /// <param name="_price"> Coste de caminar por dicho nodo </param>
        /// <param name="_gridX"> Posición X en el grid</param>
        /// <param name="_gridY"> Posición Y en el grid</param>
        public Node(float _price, int _gridX, int _gridY)
        {    
            this._walkable = _price != 0.0f;
            this._penalty = _price;
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