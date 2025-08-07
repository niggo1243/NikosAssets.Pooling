using System;
using NikosAssets.Helpers.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikosAssets.Pooling.Marker.Editor
{
    public static class EditorPoolMarkerBaker
    {
        public static void PrepareAllPoolItems<TPoolItemMarker, TPoolItemPreview, TPoolMarkerManager> 
            (Scene scene, string goName = "pool_marker", Action<TPoolItemMarker> actionOnGeneratedMarker = null) 
            where TPoolItemMarker : BasePoolItemMarker<TPoolMarkerManager, TPoolItemMarker>
            where TPoolItemPreview : BasePoolItemPreview
            where TPoolMarkerManager : BasePoolMarkerManagerWrapper<TPoolMarkerManager, TPoolItemMarker>
        {
            foreach (GameObject rootGameObject in scene.GetRootGameObjects())
            {
                foreach (TPoolItemPreview poolItemPreview in rootGameObject.GetComponentsInChildren<TPoolItemPreview>(true))
                {
                    Transform poolItemTrans = poolItemPreview.transform;
                    
                    GameObject marker = new GameObject(goName);
                    Transform markerTrans = marker.transform;
                    markerTrans.SetParent(poolItemTrans.parent);
                    markerTrans.CopyLocalTransformValuesFrom(poolItemTrans);

                    TPoolItemMarker poolItemMarker = marker.AddComponent<TPoolItemMarker>();
                    poolItemMarker.poolContainerKey = poolItemPreview.poolContainerKey;
                    
                    actionOnGeneratedMarker?.Invoke(poolItemMarker);
                    
                    EditorUtility.SetDirty(marker);
                    //now get rid of the preview!
                    GameObject.DestroyImmediate(poolItemPreview.gameObject);
                }
            }

            EditorSceneManager.MarkSceneDirty(scene);
        }
    }
}
