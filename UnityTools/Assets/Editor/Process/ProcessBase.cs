using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProcessBase : AssetPostprocessor
{
    // Start is called before the first frame update
    public static bool isUseProcess = false;
    /// <summary>
    /// 处理导入模型
    /// </summary>
    /// <param name="input"></param>
     void  OnPostprocessModel (GameObject input){
         if (isUseProcess )
         return;
         ProcessModel.OnPostprocessModel(input);
     }
    /// <summary>
    /// 处理导入图片
    /// </summary>
    /// <param name="input"></param>
    void OnPostprocessTexture(GameObject input) {
        if (isUseProcess)
            return;
    }
    void OnPreprocessAnimation(GameObject input) {
        if (isUseProcess)
            return;


    }
    void OnPreprocessAudio(GameObject input) {
        if (isUseProcess)
            return;


    }
}
