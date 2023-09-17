#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MUGCUP
{
    [CustomEditor(typeof(GridSlotData))]
    public class GridSlotDataEditor : Editor
    {
        private GridSlotData gridSlotData;

        private void OnEnable()
        {
            gridSlotData = (GridSlotData)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Highlight slots"))
            {
                gridSlotData.PlaySlotFeedbackAt(1);
            }

            if (GUILayout.Button("Generate grid slots"))
                gridSlotData.GenerateGridSlots();
            
            if (GUILayout.Button("Clear"))
                gridSlotData.ClearGridSlots();
        }
    }
}
#endif
