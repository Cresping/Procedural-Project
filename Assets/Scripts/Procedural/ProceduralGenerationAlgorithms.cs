using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    /// <summary>
    /// Clase encargada de almacenar los distintos algoritmos de generaci�n procedural
    /// </summary>
    public static class ProceduralGenerationAlgorithms
    {
        /// <summary>
        /// Algoritmo de generaci�n procedural BSP
        /// </summary>
        /// <param name="spaceToSplit">Mapa que se dividir�</param>
        /// <param name="minWidth">M�nima anchura de las habitaciones</param>
        /// <param name="minHeight">M�xima anchura de las habitaciones</param>
        /// <returns></returns>
        public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
        {
            Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
            List<BoundsInt> roomsList = new List<BoundsInt>();
            roomsQueue.Enqueue(spaceToSplit);
            while (roomsQueue.Count > 0)
            {
                var room = roomsQueue.Dequeue();
                if (room.size.y >= minHeight && room.size.x >= minWidth)
                {
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        if (room.size.y >= minHeight * 2)
                        {
                            SplitHorizontal(minHeight, roomsQueue, room);
                        }
                        else if (room.size.x >= minWidth * 2)
                        {
                            SplitVertical(minWidth, roomsQueue, room);
                        }
                        else if (room.size.x >= minWidth && room.size.y >= minHeight)
                        {
                            roomsList.Add(room);
                        }
                    }
                    else
                    {
                        if (room.size.x >= minWidth * 2)
                        {
                            SplitVertical(minWidth, roomsQueue, room);
                        }
                        else if (room.size.y >= minHeight * 2)
                        {
                            SplitHorizontal(minHeight, roomsQueue, room);
                        }
                        else if (room.size.x >= minWidth && room.size.y >= minHeight)
                        {
                            roomsList.Add(room);
                        }
                    }
                }
            }
            return roomsList;
        }
        /// <summary>
        /// Divide una habitacion verticalmente y la agrega a la cola de habitaciones
        /// </summary>
        /// <param name="minWidth">M�nima anchura de la habitaci�n</param>
        /// <param name="roomsQueue">Cola de habitaciones</param>
        /// <param name="room">Habitaci�n que se dividir�</param>
        private static void SplitVertical(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            var xSplit = Random.Range(1, room.size.x);
            BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
            BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
                new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }
        /// <summary>
        /// Divide una habitacion horizontalmente y la agrega a la cola de habitaciones
        /// </summary>
        /// <param name="minWidth">M�nima anchura de la habitaci�n</param>
        /// <param name="roomsQueue">Cola de habitaciones</param>
        /// <param name="room">Habitaci�n que se dividir�</param>
        private static void SplitHorizontal(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
        {
            var ySplit = Random.Range(1, room.size.y);
            BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
            BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
                new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
            roomsQueue.Enqueue(room1);
            roomsQueue.Enqueue(room2);
        }
    }
}

