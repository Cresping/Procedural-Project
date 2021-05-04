using UnityEngine;

namespace HeroesGames.ProjectProcedural.Pathfind
{
    /// <summary>
    /// Clase encargada de almcenar los datos de un punto del mapa
    /// </summary>
    public class Point
    {
        private int posX;
        private int posY;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Point()
        {
            posX = 0;
            posY = 0;
        }

        /// <summary>
        /// Constructor parametrizado
        /// </summary>
        /// <param name="iX"></param>
        /// <param name="iY"></param>
        public Point(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
        }
        public Vector2Int Position
        {
            get
            {
                return new Vector2Int(this.posX, this.posY);
            }
            set
            {
                posX = value.x;
                posY = value.y;
            }
        }

        public int PosX { get => posX; set => posX = value; }
        public int PosY { get => posY; set => posY = value; }
    }
}