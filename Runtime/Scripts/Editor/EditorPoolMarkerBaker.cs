using System;
using NikosAssets.Helpers.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NikosAssets.Pooling.Marker.Editor
{
    /// <summary>
    /// An editor helper class to automate pre-placed poolable items
    /// </summary>
    public static class EditorPoolMarkerBaker
    {
        /// <summary>
        /// Deletes and replaces <typeparamref name="TPoolItemPreview"/> gameObjects with <typeparamref name="TPoolItemMarker"/> gameObjects
        /// and automatically sets the pool key for the given scene on all found active and inactive gameObjects
        /// </summary>
        /// <param name="scene">
        /// Searches in this given scene
        /// </param>
        /// <param name="goName">
        /// How should the marker be called?
        /// </param>
        /// <param name="actionOnGeneratedMarker">
        /// Optional action to perform on the newly generated marker
        /// </param>
        /// <typeparam name="TPoolItemMarker"></typeparam>
        /// <typeparam name="TPoolItemPreview"></typeparam>
        /// <typeparam name="TPoolMarkerManager"></typeparam>
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
