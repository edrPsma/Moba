using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HybridCLR.Editor.Settings;
using System.IO;

public class CopyDll : MonoBehaviour
{
    const string AssemblyPath = "Assets/GameAssets/Dll";
    const string AotPath = "Assets/GameAssets/Dll/Aot";
    const string ProtocolPath = "Assets/Plugins/Protocol/netstandard2.0/Protocol.dll";

    [MenuItem("Tools/Copy Dlls")]
    static void Copy()
    {
        EditorApplication.ExecuteMenuItem("HybridCLR/CompileDll/ActiveBuildTarget");
        List<string> files = HybridCLR.Editor.SettingsUtil.HotUpdateAssemblyNamesExcludePreserved;

        string assemblyPath = $"{Application.dataPath.Replace("Assets", "")}{HybridCLR.Editor.SettingsUtil.HotUpdateDllsRootOutputDir}/{EditorUserBuildSettings.activeBuildTarget}";
        string aotPath = $"{Application.dataPath.Replace("Assets", "")}{HybridCLR.Editor.SettingsUtil.AssembliesPostIl2CppStripDir}/{EditorUserBuildSettings.activeBuildTarget}";
        string assemblyOutPutPath = $"{Application.dataPath.Replace("Assets", "")}{AssemblyPath}";
        string aotOutPutPath = $"{Application.dataPath.Replace("Assets", "")}{AotPath}";
        if (!Directory.Exists(assemblyPath)) return;
        if (!Directory.Exists(aotPath)) return;
        if (!Directory.Exists(assemblyOutPutPath))
        {
            Directory.CreateDirectory(assemblyOutPutPath);
        }
        if (!Directory.Exists(aotOutPutPath))
        {
            Directory.CreateDirectory(aotOutPutPath);
        }

        for (int i = 0; i < files.Count; i++)
        {
            string filePath = $"{assemblyPath}/{files[i]}.dll";
            if (!File.Exists(filePath)) continue;

            string newFilePath = $"{assemblyOutPutPath}/{files[i]}.dll.bytes";

            File.Copy(filePath, newFilePath, true);
        }

        foreach (var item in AOTGenericReferences.PatchedAOTAssemblyList)
        {
            string filePath = $"{aotPath}/{item}";
            if (!File.Exists(filePath)) continue;

            string newFilePath = $"{aotOutPutPath}/{item}.bytes";

            File.Copy(filePath, newFilePath, true);
        }

        string newProtocolPath=$"{assemblyOutPutPath}/Protocol.dll.bytes";
        File.Copy(ProtocolPath, newProtocolPath, true);


        AssetDatabase.Refresh();
        Debug.Log(string.Format("<color=green>{0}</color>", "生成成功"));
    }
}
