#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    /// <summary>
    /// Clase que permite pintar la mazmorra entera sin tener que entrar en modo ejecucción
    /// </summary>
    [CustomEditor(typeof(AbstractDoungeonGenerator), true)]
    public class RandomDungeonGeneratorInspector : Editor
    {
        private AbstractDoungeonGenerator _generator;

        private void Awake()
        {
            _generator = (AbstractDoungeonGenerator)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Dungeon"))
            {
                _generator.GenerateDungeon();
            }
        }
    }
}
#endif

