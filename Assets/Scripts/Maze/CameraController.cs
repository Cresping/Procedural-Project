using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private MazeGenerator maze;

        public void SetInitialCameraPosition(int indexCorner)
        {
            var cameraTransform = transform;
            cameraTransform.position = indexCorner switch
            {
                0 => new Vector3(2, 4, -5),
                1 => new Vector3(2, maze.Height - 4, -5),
                2 => new Vector3(maze.Width - 2, maze.Height - 4, -5),
                3 => new Vector3(maze.Width - 2, 4, -5),
                _ => cameraTransform.position
            };
        }
    }
}

