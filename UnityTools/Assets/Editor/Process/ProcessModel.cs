using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ProcessModel {

    public static void OnPostprocessModel (GameObject input) {
        // 设置导入模型的tag
        //input.tag = "";
        return;
        // 取得导入模型的相关信息
        ModelImporter importer = assetImporter as ModelImporter;

        AssetDatabase.Refresh ();

        // 从工程中将该模型读出来
        GameObject tar = AssetDatabase.LoadAssetAtPath (importer.assetPath, typeof (GameObject)) as GameObject;

        if (tar) {
            GameObject obj = GameObject.Instantiate (tar);

            Texture2D textureData = AssetDatabase.LoadAssetAtPath (("Assets/BundleResources/Texture/" + input.name + ".png"), typeof (Texture2D)) as Texture2D;

            if (textureData) {
                Material importMat = new Material (Shader.Find ("Mobile/Unlit (Supports Lightmap)"));
                importMat.mainTexture = textureData;
                AssetDatabase.CreateAsset (importMat, "Assets/BundleResources/Materials/" + input.name + ".mat");

                Material[] materials = {
                    AssetDatabase.LoadAssetAtPath (("Assets/BundleResources/Materials/" + input.name + ".mat"), typeof (Material)) as Material
                };
                obj.GetComponent<MeshRenderer> ().materials = materials;
                // 将这个模型创建为Prefab
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset (obj, ("Assets/BundleResources/Prefabs/" + input.name + ".Prefab"));
            }
        }
    }
}