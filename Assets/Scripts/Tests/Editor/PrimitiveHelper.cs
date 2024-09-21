using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests.Editor
{
    public static class PrimitiveHelper
    {
        private static readonly Dictionary<PrimitiveType, Mesh> Meshes =
            new Dictionary<PrimitiveType, Mesh>();

        public static Mesh GetPrimitiveMesh(PrimitiveType type)
        {
            Debug.Log($"{nameof(PrimitiveHelper)}.{nameof(GetPrimitiveMesh)}({type})");

            return Meshes[type];
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            Debug.Log($"{nameof(PrimitiveHelper)}.{nameof(Initialize)}()");
            Debug.Log($"{nameof(Meshes)}: {Meshes.Count}");

            EditorApplication.update += OnNextUpdate;

            void OnNextUpdate()
            {
                EditorApplication.update -= OnNextUpdate;
                RetrieveMeshes();
                Debug.Log($"{nameof(Meshes)}: {Meshes.Count}");
            }
        }

        private static void RetrieveMeshes()
        {
            Debug.Log($"{nameof(PrimitiveHelper)}.{nameof(RetrieveMeshes)}()");

            var scene = EditorSceneManager.NewPreviewScene();

            var primitiveTypes = Enum.GetValues(typeof(PrimitiveType))
                .Cast<PrimitiveType>();

            foreach (var primitiveType in primitiveTypes)
            {
                var gameObject = GameObject.CreatePrimitive(primitiveType);
                EditorSceneManager.MoveGameObjectToScene(gameObject, scene);

                var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
                Meshes[primitiveType] = mesh;
            }

            EditorSceneManager.ClosePreviewScene(scene);
        }
    }
}
