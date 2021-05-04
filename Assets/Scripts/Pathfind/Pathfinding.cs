using UnityEngine;
using System.Collections.Generic;

namespace HeroesGames.ProjectProcedural.Pathfind
{
    /// <summary>
    /// Clase encargada de encontrar un camino, si es posible, entre dos posiciones
    /// </summary>
    public class Pathfinding
    {
        /// <summary>
        /// Devuelve la lista de puntos que se deben de recorrer para llegar a la posición deseada
        /// </summary>
        /// <param name="grid"> El conjunto de nodos</param>
        /// <param name="startPos">Posición inicial</param>
        /// <param name="targetPos">Posición final</param>
        /// <returns></returns>
        public static List<Point> FindPath(GridPathfind grid, Point startPos, Point targetPos)
        {
            List<Node> nodes_path = ImpFindPath(grid, startPos, targetPos);

            List<Point> ret = new List<Point>();
            if (nodes_path != null)
            {
                foreach (Node node in nodes_path)
                {
                    ret.Add(new Point(node.GridX, node.GridY));
                }
            }
            return ret;
        }

        /// <summary>
        /// Devuelve la lista de nodos que se deben de recorrer para llegar a la posición deseada
        /// </summary>
        /// <param name="grid">El conjunto de nodos</param>
        /// <param name="startPos">Posición inicial</param>
        /// <param name="targetPos">Posición final</param>
        /// <returns></returns>
        private static List<Node> ImpFindPath(GridPathfind grid, Point startPos, Point targetPos)
        {
            Node startNode = grid.GetNode(startPos.PosX,startPos.PosY);
            Node targetNode = grid.GetNode(targetPos.PosX, targetPos.PosY);

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return RetracePath(grid, startNode, targetNode);
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode,false))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.Penalty);
                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Módulo encargado de volver atras en la lista de nodos
        /// </summary>
        /// <param name="grid">El conjunto de nodos</param>
        /// <param name="startNode">Nodo de inicio</param>
        /// <param name="endNode">Nodo final</param>
        /// <returns></returns>
        private static List<Node> RetracePath(GridPathfind grid, Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }
        /// <summary>
        /// Devuelve la distancia entre dos nodos
        /// </summary>
        /// <param name="nodeA">Nodo A</param>
        /// <param name="nodeB">Nodo B</param>
        /// <returns>Distancia entera entre los dos nodos</returns>
        private static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

}