using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.Utils;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = nameof(BSPDungeonVariableSO), menuName = "Scriptables/" + nameof(BSPDungeonVariableSO) + "/" + nameof(BSPDungeonVariableSO) + "Variable")]
    public class BSPDungeonVariableSO : DungeonVariableSO, ISerializationCallbackReceiver
    {
        [SerializeField] private GameVariableSO gameVariableSO;
        [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
        [SerializeField] [Range(0, 10)] private int offset = 1;
        private List<RoomVariableSO> _roomVariableSOs;
        private List<RoomVariableSO> _selectedRooms;

        public int MinRoomHeight { get => minRoomHeight; set => minRoomHeight = value; }
        public int MinRoomWidth { get => minRoomWidth; set => minRoomWidth = value; }
        public int Offset { get => offset; set => offset = value; }
        public List<RoomVariableSO> SelectedRooms { get => _selectedRooms; set => _selectedRooms = value; }

        private void LoadAllRoomVariables()
        {
            RoomVariableSO[] roomArray;
            _roomVariableSOs = new List<RoomVariableSO>();
            roomArray = Resources.LoadAll<RoomVariableSO>("SO/Rooms");
            foreach (RoomVariableSO room in roomArray)
            {
                _roomVariableSOs.Add(room);
            }
            Debug.Log("Hay un total de " + _roomVariableSOs.Count + " habitaciones");
        }
        private void GenerateSelectedRooms()
        {
            EnumTypes.RoomType selectedRoomType;
            _selectedRooms = new List<RoomVariableSO>();
            if (dungeonLvl < gameVariableSO.MaxLevelEasy)
            {
                selectedRoomType = EnumTypes.RoomType.Easy;
            }
            else if (dungeonLvl < gameVariableSO.MaxLevelNormal)
            {
                selectedRoomType = EnumTypes.RoomType.Normal;
            }
            else if (dungeonLvl < gameVariableSO.MaxLevelHard)
            {
                selectedRoomType = EnumTypes.RoomType.Hard;
            }
            else if (dungeonLvl < gameVariableSO.MaxLevelVeryHard)
            {
                selectedRoomType = EnumTypes.RoomType.VeryHard;
            }
            else
            {
                selectedRoomType = EnumTypes.RoomType.Lunatic;
            }
            foreach (RoomVariableSO room in _roomVariableSOs)
            {
                if (room.RoomType == selectedRoomType)
                {
                    _selectedRooms.Add(room);
                }
            }
            _selectedRooms = ListUtils.UnsortList<RoomVariableSO>(_selectedRooms);
        }
        private void GenerateDungeonSize()
        {
            dungeonWidth = 20 + dungeonLvl * gameVariableSO.DifficultyParameter;
            dungeonHeight = 20 + dungeonLvl * gameVariableSO.DifficultyParameter;
        }
        public void GenerateRoomSize()
        {
            int randomHeigh = 0;
            int randomWidth = 0;
            if (dungeonLvl < gameVariableSO.MaxLevelNormal)
            {
                randomHeigh = Random.Range(0, 2);
                randomWidth = Random.Range(0, 2);
            }
            else if (dungeonLvl < gameVariableSO.MaxLevelHard)
            {
                randomHeigh = Random.Range(1, 3);
                randomWidth = Random.Range(1, 3);
            }
            else if (dungeonLvl < gameVariableSO.MaxLevelVeryHard)
            {
                randomHeigh = Random.Range(2, 5);
                randomWidth = Random.Range(2, 5);
            }
            else if (dungeonLvl >= gameVariableSO.MaxLevelVeryHard)
            {
                randomHeigh = Random.Range(3, 6);
                randomWidth = Random.Range(3, 6);
            }
            minRoomHeight = 4 + randomHeigh;
            minRoomWidth = 4 + randomWidth;
        }
        private void OnEnable()
        {
            LoadAllRoomVariables();
        }
        public override void CalculateDifficulty()
        {
            GenerateDungeonSize();
            GenerateRoomSize();
            GenerateSelectedRooms();
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }
    }
}

