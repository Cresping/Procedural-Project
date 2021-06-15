using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class MazeGenerator : AbstractDoungeonGenerator
    {
        #region VARIABLES
    
        [SerializeField]
        private int
            width,
            height;

        private List<Vector2Int> CurrentFrontiers = new List<Vector2Int>();
        private List<Vector2Int> MazePaths = new List<Vector2Int>();
    
        private bool[,] _maze; // false = is maze's corridor, true = is NOT maze's corridor
    
        #endregion

        #region INIT MAZE

        protected override void RunProceduralGeneration()
        {
            CleanContents();
            InitMaze();
            PrimsAlgorithm();
            DrawTilesInMaze();
            DrawBorders();
        }

        // Initialize the maze array.
        private void InitMaze()
        {
            _maze = new bool[width, height];
            for (var column = 0; column < width; column++)
            for (var row = 0; row < height; row++)
                _maze[column, row] = true;
        }
        
        // Restore Lists and Array
        private void CleanContents()
        {
            CurrentFrontiers.Clear();
            MazePaths.Clear();
            _maze = null;
        }
        
        #endregion
        
        #region PRIM'S ALGORITHM

        // We use Prim's algorithm to generate corridors in the maze
        private void PrimsAlgorithm()
        {
            // 1. Choose random initial node, add to corridors and add it to Frontiers List
            var randomX = Mathf.Clamp(Random.Range(0, width) * 2,0,width -1);
            var randomY = Mathf.Clamp(Random.Range(0, height) * 2,0, height -1);

            _maze[randomX, randomY] = false;
            AddFrontiers(new Vector2Int(randomX, randomY));

            // 2. Repeat until there aren't frontiers in CurrentFrontiers list
            while (CurrentFrontiers.Count != 0)
            {
                // 2a. Select a random frontier from list
                var randomIndex = Random.Range(0, CurrentFrontiers.Count - 1);
                var chosenFrontier = CurrentFrontiers[randomIndex];

                // 2b. Choose a random node that isn't a corridor of the maze and creates a floor toward it
                var chosenNeighbour = ChooseRandomNeighbour(new Vector2Int(chosenFrontier.x, chosenFrontier.y));
            
                // 2c. Turns the selected frontier and selected neighbour into a maze corridor and add them to MazePaths list
                _maze[chosenFrontier.x, chosenFrontier.y] = false;                

                _maze[chosenNeighbour.x, chosenNeighbour.y] = false;                

                // 2d. Turns the node between the selected frontier and the new maze's neighbour into a maze corridor
                LinkFloorNodes(new Vector2Int(chosenFrontier.x, chosenFrontier.y), new Vector2Int(chosenNeighbour.x, chosenNeighbour.y));

                // 2e. Add new frontier tiles to CurrentFrontiers list
                AddFrontiers(new Vector2Int(chosenFrontier.x, chosenFrontier.y));

                // 2f. Remove the chosen frontier from our CurrentFrontiers list
                CurrentFrontiers.Remove(chosenFrontier);
            }

            // 3. Add every corridor in the Maze to MazePaths list to make them walkable
            for (var column = 0; column < width; column++)
                for (var row = 0; row < height; row++)
                {
                    if(!_maze[column,row])
                        AddNodeToMazePaths(new Vector2Int(column, row));
                }
        }

        // Add nodes to MazePath list => transitable tiles for player and enemies
        private void AddNodeToMazePaths(Vector2Int position) => MazePaths.Add(position);

        // Add available nodes to our frontier list        
        private void AddFrontiers(Vector2Int position)
        {
            if (position.x - 2 >= 0 && _maze[position.x - 2, position.y])
                CurrentFrontiers.Add(new Vector2Int(position.x - 2, position.y));

            if (position.x + 2 < width && _maze[position.x + 2, position.y])
                CurrentFrontiers.Add(new Vector2Int(position.x + 2, position.y));

            if (position.y - 2 >= 0 && _maze[position.x, position.y - 2])
                CurrentFrontiers.Add(new Vector2Int(position.x, position.y - 2));

            if (position.y + 2 < height && _maze[position.x, position.y + 2])
                CurrentFrontiers.Add(new Vector2Int(position.x, position.y + 2));
        }

        // Choose and return a random no corridor neighbour        
        private Vector2Int ChooseRandomNeighbour(Vector2Int position)
        {
            var neighbours = new List<Vector2Int>();

            if (position.x - 2 >= 0 && !_maze[position.x - 2, position.y])
                neighbours.Add(new Vector2Int(position.x - 2, position.y));

            if (position.x + 2 < width && !_maze[position.x + 2, position.y])
                neighbours.Add(new Vector2Int(position.x + 2, position.y));

            if (position.y - 2 >= 0 && !_maze[position.x, position.y - 2])
                neighbours.Add(new Vector2Int(position.x, position.y - 2));

            if (position.y + 2 < height && !_maze[position.x, position.y + 2])
                neighbours.Add(new Vector2Int(position.x, position.y + 2));

            var randomNeighbour = Random.Range(0, neighbours.Count - 1);

            return neighbours[randomNeighbour];
        }

        // Remove the wall tile between two chosen in maze floors to complete the corridor
        private void LinkFloorNodes(Vector2Int startPosition, Vector2Int endPosition)
        {
            if (startPosition.x == endPosition.x)
            {
                var minRow = Mathf.Min(startPosition.y, endPosition.y);
                _maze[startPosition.x, minRow + 1] = false;
            }
            else if (startPosition.y == endPosition.y)
            {
                var minColumn = Mathf.Min(startPosition.x, endPosition.x);
                _maze[minColumn + 1, startPosition.y] = false;
            }
        }

        #endregion

        #region DRAW THE MAZE

        // Draw Tiles in maze depends on _maze values
        private void DrawTilesInMaze()
        {
            for (var column = 0; column < width; column++)
            for (var row = 0; row < height; row++)
            {
                if (_maze[column, row])
                    tileMapGenerator.PaintWallTile(new Vector2Int(column,row));
                else
                    tileMapGenerator.PaintFloorTile(new Vector2Int(column, row));
            }

            //Draw Exit
            var randomPathTile = MazePaths[Random.Range(0, MazePaths.Count - 1)];
            tileMapGenerator.PaintEndTile(randomPathTile);
        }
    
        // Draw Borders
        private void DrawBorders()
        {
            for (var column = 0; column < width; column++)
            for (var row = 0; row < height; row++)
            {
                    //Draw left and right boundaries
                    if (column == 0)
                        tileMapGenerator.PaintFenceTile(new Vector2Int(column - 1, row));
                    else if (column == width - 1)
                        tileMapGenerator.PaintFenceTile(new Vector2Int(width, row));

                    //Draw up and down boundary
                    if (row == 0)
                        tileMapGenerator.PaintFenceTile(new Vector2Int(column, row - 1));
                    else if (row == height - 1)
                        tileMapGenerator.PaintFenceTile(new Vector2Int(column, height));   
            }

            // Draw corners
            tileMapGenerator.PaintFenceTile(new Vector2Int(-1, -1));
            tileMapGenerator.PaintFenceTile(new Vector2Int(-1, height));
            tileMapGenerator.PaintFenceTile(new Vector2Int(width, -1));
            tileMapGenerator.PaintFenceTile(new Vector2Int(width, height));
        }

        #endregion
    }
}