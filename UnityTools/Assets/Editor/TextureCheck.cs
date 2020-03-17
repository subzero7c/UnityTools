using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Code by zhanzhang
/// </summary>
public class TextureCheck : AssetPostprocessor {
    //纹理导入器, pre-process
    void OnPreprocessTexture() {
        //获得importer实例
        TextureImporter tImporter = assetImporter as TextureImporter;

        //设置Read/Write Enabled开关,不勾选
        tImporter.isReadable = false;

        if (tImporter.assetPath.StartsWith("Assets/UITextures")) {
            //设置UI纹理Generate Mipmaps开关,不勾选
            tImporter.mipmapEnabled = false;
            //设置UI纹理WrapMode开关,Clamp
            tImporter.wrapMode = TextureWrapMode.Clamp;
        }

        //设置压缩格式
        TextureImporterPlatformSettings psAndroid = tImporter.GetPlatformTextureSettings("Android");
        TextureImporterPlatformSettings psIPhone = tImporter.GetPlatformTextureSettings("iPhone");
        psAndroid.overridden = true;
        psIPhone.overridden = true;
        if (tImporter.DoesSourceTextureHaveAlpha()) {
            psAndroid.format = TextureImporterFormat.ETC2_RGBA8;
            psIPhone.format = TextureImporterFormat.ASTC_RGBA_4x4;
        }
        else {
            psAndroid.format = TextureImporterFormat.ETC2_RGB4;
            psIPhone.format = TextureImporterFormat.ASTC_RGB_4x4;
        }
        tImporter.SetPlatformTextureSettings(psAndroid);
        tImporter.SetPlatformTextureSettings(psIPhone);
    }

    [MenuItem("校验工具/Session01")]
    static public void AutoValidate() {
        int progress = 0;
        int length = 0;
        //写入csv日志
        StreamWriter sw = new StreamWriter("ValidateS01.csv", false, System.Text.Encoding.UTF8);
        sw.WriteLine("Validate -- Session01");

        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        length = allAssets.Length;
        EditorUtility.DisplayProgressBar("检测图片", "检查资源中，请耐心等候", progress / length);
        foreach (string s in allAssets) {
            progress += 1;
            if (s.StartsWith("Assets/BundleResources")) {
                Texture tex = AssetDatabase.LoadAssetAtPath(s, typeof(Texture)) as Texture;
                if (tex) {
                    Debug.Log("进入内部");
                    //检测纹理资源命名是否合法
                    if (!Regex.IsMatch(s, @"^[a-zA-Z][a-zA-Z0-9_/.]*$")) {
                        sw.WriteLine(string.Format("图片名称不符合规范,{0}", s));
                    }

                    //判断纹理尺寸是否符合四的倍数
                    if (((tex.width % 4) != 0) || ((tex.height % 4) != 0)) {
                        sw.WriteLine(string.Format("图片纹理不符合4的倍数,{0},{1},{2}", s, tex.width, tex.height));
                    }
                    if (tex.width > 1024 || tex.height > 1024) {
                        sw.WriteLine(string.Format("图片长宽过大,{0},{1},{2}", s, tex.width, tex.height));
                    }
                }
            }
        }
        EditorUtility.ClearProgressBar();
        sw.Flush();
        sw.Close();
    }
}
