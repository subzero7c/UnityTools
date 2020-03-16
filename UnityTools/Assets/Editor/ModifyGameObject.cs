using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
public class ModifyGameObject {

    [MenuItem("StarUnion/Zhanzhang's Tools", false, 10)]
    static private void Find() {
        EditorSettings.serializationMode = SerializationMode.ForceText;
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.Assets | SelectionMode.ExcludePrefab);
        //此处添加需要命名的资源后缀名,注意大小写。		
        string[] Filtersuffix = new string[] { ".prefab", ".mat", ".dds", ".png", ".jpg", ".shader", ".csv", ".wav", ".mp3" };
        if (SelectedAsset.Length == 0) return;
        foreach (Object tmpFolder in SelectedAsset) {
            string path = AssetDatabase.GetAssetPath(tmpFolder);
            if (!string.IsNullOrEmpty(path)) {
                string guid = AssetDatabase.AssetPathToGUID(path);
                List<string> withoutExtensions = new List<string>() { ".prefab", ".unity", ".mat", ".asset" };
                string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                    .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
                int num = 0;
                for (var i = 0; i < files.Length; ++i) {
                    string file = files[i]; //显示进度条			
                    EditorUtility.DisplayProgressBar("匹配资源", "正在匹配资源中...", 1.0f * i / files.Length);
                    if (Regex.IsMatch(File.ReadAllText(file), guid)) {
                        Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
                        num++;
                    }
                }
                if (num == 0) {
                    Debug.LogError(tmpFolder.name + "     匹配到" + num + "个", tmpFolder);
                }
                else if (num == 1) {
                    Debug.Log(tmpFolder.name + "     匹配到" + num + "个", tmpFolder);
                }
                else {
                    Debug.LogWarning(tmpFolder.name + "     匹配到" + num + "个", tmpFolder);
                }
                num = 0;
                //				int startIndex = 0;
                //		                EditorApplication.update = delegate() {
                //					string file = files [startIndex];
                ////					bool isCancel = EditorUtility.DisplayCancelableProgressBar ("匹配资源中", file, (float)startIndex / (float)files.Length);////					if (Regex.IsMatch (File.ReadAllText (file), guid)) {//						Debug.Log (file, AssetDatabase.LoadAssetAtPath<Object> (GetRelativeAssetsPath (file)));//					}////					startIndex++;//					if (isCancel || startIndex >= files.Length) {//						//						EditorApplication.update = null;//						startIndex = 0;//						Debug.Log ("匹配结束" + tmpFolder.name);//					}////				};			
            }
        }

        EditorUtility.ClearProgressBar();
    }

    [MenuItem("StarUnion/Zhanzhang's Tools/ModiFy", true)]
    static private bool VFind() {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return (!string.IsNullOrEmpty(path));
    }
    static private string GetRelativeAssetsPath(string path) { return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/'); }

    [MenuItem("StarUnion/Zhanzhang's Tools/CheckAllMaterial", false, 10)]
    ///打开修改材质界面
    static void ShowMaterialModify() {

    }

}

public class ModifyGameObjectWindow : EditorWindow {
    static List<Material> allMat = new List<Material>();
    static Dictionary<string, List<Material>> allMatMap = new Dictionary<string, List<Material>>();

    static private void FindMatrial() {
        string[] allPath = AssetDatabase.FindAssets("t:Material");
        for (int i = 0; i < allPath.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(allPath[i]);
            var obj = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
            allMat.Add(obj);
            //if (allMatMap.ContainsKey(obj.tag)) {
            //}

        }

    }

    [MenuItem("StarUnion/Zhanzhang's Tools/ModifyRenderQueue", false, 10)]
    static void Init() {
        ModifyGameObjectWindow modifyWindow = (ModifyGameObjectWindow)EditorWindow.GetWindow(typeof(ModifyGameObjectWindow));
        modifyWindow.Show();
        FindMatrial();
    }

    void OnGUI() {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("材质球管理界面");
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        for (int i = 0; i < allMat.Count; i++) {
            var mat = allMat[i];
            GUILayout.Space(10);

            //GUILayout.Label(mat.name);
            Material buggyGameObject = null;

            buggyGameObject = (Material)EditorGUILayout.ObjectField(mat.name, buggyGameObject, typeof(Material), true);
        }
        //绘制文本
        GUILayout.Space(10);
        //bugReporterName = EditorGUILayout.TextField("Bug Name", bugReporterName);

        //绘制当前正在编辑的场景
        GUILayout.Space(10);
        
        //GUILayout.Label("Currently Scene:" + EditorSceneManager.GetActiveScene().name);



        //绘制对象
        GUILayout.Space(10);

        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //添加名为"Save Bug"按钮，用于调用SaveBug()函数
        //if (GUILayout.Button("保存所有修改")) {
        //    GUIUtility.ExitGUI();
        //}


        GUILayout.EndVertical();
        GUIUtility.ExitGUI();
    }

}

public class MaterialInfo : ScriptableObject {
    public string MaterialName;
    public string TagName;
    public int RendererQueue;



}