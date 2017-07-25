// ===========
// DO NOT EDIT - this file is automatically regenerated.
// =========== 
using Improbable.Unity.CodeGeneration;
using Improbable.Unity.EditorTools.Internal;
using Improbable.Unity.EditorTools.Util;
using UnityEditor;
using UnityEngine;

namespace Improbable
{
    [CustomEditor(typeof(SpatialOsMetadataComponent))]
    public class SpatialOsMetadataComponentEditor : UnityEditor.Editor
    {
        private SpatialOsMetadataComponent editedComponent;
        private SpatialOsMetadataComponentEditorData componentData;

        private class SpatialOsMetadataComponentEditorData : SpatialOsComponentEditorBase<SpatialOsMetadataComponent>
        {
            public string EntityType
            {
                get { return Component.EntityType; }
            }
        }

        protected virtual void OnEnable()
        {
            editedComponent = (SpatialOsMetadataComponent) target;

            var hashCode = editedComponent.GetHashCode();
            componentData = (SpatialOsMetadataComponentEditorData) EditorObjectStateManager.GetComponentEditorData(hashCode);
            if(componentData == null)
            {
                componentData = new SpatialOsMetadataComponentEditorData();
                EditorObjectStateManager.SetComponentEditorData(hashCode, componentData);
            }
            
            componentData.AttachComponent(editedComponent);
        }

        protected virtual void OnDisable()
        {
            componentData.DetachComponent();
        }

        private Vector2 scrollPos;

        public override void OnInspectorGUI()
        {
            componentData.UpdateEditorLogs();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("EntityId", componentData.EntityId.IsValid() ? componentData.EntityId.Id.ToString() : "n/a");
            if (componentData.IsComponentReady)
            {
                EditorGUILayout.LabelField("Component Initialised", GuiUtil.GreenTextStyle);
            }
            else
            {
                EditorGUILayout.LabelField("Component Not Initialised", GuiUtil.RedTextStyle);
            }

            if (componentData.HasAuthority)
            {
                EditorGUILayout.LabelField("Authoritative", GuiUtil.GreenTextStyle);
            }
            else
            {
                EditorGUILayout.LabelField("Not Authoritative", GuiUtil.RedTextStyle);
            }

            EditorGUILayout.Space();
            EditorGUILayout.TextField("entityType", componentData.EntityType);
 

            EditorGUILayout.Space();
            componentData.ShowUpdates = EditorGUILayout.Foldout(componentData.ShowUpdates, string.Format("Component Updates: ({0}/s)",
                System.Math.Round(componentData.AverageComponentUpdatesPerSecond, 2)), /* toggleOnLabelClick: */ true );

            if (componentData.ShowUpdates)
            {
                scrollPos = EditorGUILayout.BeginScrollView(
                    scrollPos, /* always horizontal scroll bar */ false, /* always vertical scroll bar */ false, 
                    GUILayout.Width(100), GUILayout.Height(100));
                if (componentData.ComponentUpdateLogArray != null)
                {
                    for (var i = 0; i < componentData.ComponentUpdateLogArray.Length; i++)
                    {
                        GUILayout.Label(componentData.ComponentUpdateLogArray[i]);
                    }
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.Space();
            }

            componentData.ShowCommands = EditorGUILayout.Foldout(componentData.ShowCommands, string.Format("Command Requests: ({0}/s)",
                System.Math.Round(componentData.AverageCommandRequestsPerSecond, 2)), /* toggleOnLabelClick: */ true);

            if (componentData.ShowCommands)
            {
                scrollPos = EditorGUILayout.BeginScrollView(
                    scrollPos, /* always horizontal scroll bar */ false, /* always vertical scroll bar */ false,
                    GUILayout.Width(100), GUILayout.Height(100));

                if (componentData.CommandRequestLogArray != null)
                {
                    for (var i = 0; i < componentData.CommandRequestLogArray.Length; i++)
                    {
                        GUILayout.Label(componentData.CommandRequestLogArray[i]);
                    }
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }
}

