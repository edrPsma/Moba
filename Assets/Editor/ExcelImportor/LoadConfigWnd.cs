using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.IO;
using Sirenix.Utilities.Editor;
using System.Text;
using System.Linq;
using Excel;

public class LoadConfigWnd : Sirenix.OdinInspector.Editor.OdinEditorWindow
{
    public static LoadConfigWnd Instance;
    const string PrefsKey = "ConfigPath";
    const string FocusPrefsKey = "FocusConfigPath";

    [Header("配置")]
    [FolderPath(RequireExistingPath = true, ParentFolder = "Assets", AbsolutePath = true)]
    [OnValueChanged(nameof(OnConfigPathChange))]
    public string ConfigPath;

    // public bool LoadAndCommit;

    [Header("数据")]
    [ListDrawerSettings(OnTitleBarGUI = nameof(DrawButtonGroup), NumberOfItemsPerPage = 10, IsReadOnly = true)]
    [Searchable]
    public List<LoadInfo> LoadInfos = new List<LoadInfo>();

    [Header("关注列表")]
    [ListDrawerSettings(NumberOfItemsPerPage = 10, IsReadOnly = true, ShowFoldout = true)]
    // [Searchable]
    public List<LoadInfo> FocusInfos = new List<LoadInfo>();

    [MenuItem("Tools/配置表导入")]
    static void OpenWnd()
    {
        var window = GetWindow<LoadConfigWnd>();

        if (EditorPrefs.HasKey(PrefsKey))
        {
            window.ConfigPath = EditorPrefs.GetString(PrefsKey);
            window.LoadIgnoreList();
            window.GetAllLoadInfo();
        }

        Instance = window;
        window.Show();
    }

    void OnConfigPathChange(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        EditorPrefs.SetString(PrefsKey, path);

        GetAllLoadInfo();
    }

    void OnIgnoreConfigListChange()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in FocusInfos)
        {
            sb.AppendLine(item.Path);
        }

        EditorPrefs.SetString(FocusPrefsKey, sb.ToString());
    }

    void LoadIgnoreList()
    {
        if (string.IsNullOrEmpty(ConfigPath)) return;

        FocusInfos.Clear();
        if (!EditorPrefs.HasKey(FocusPrefsKey)) return;

        string[] paths = EditorPrefs.GetString(FocusPrefsKey).Split('\n', '\r');
        foreach (var item in paths)
        {
            if (!File.Exists(item)) continue;

            FileInfo fileInfo = new FileInfo(item);

            LoadInfo loadInfo = new LoadInfo
            {
                IsLoad = false,
                Path = fileInfo.FullName,
                Name = fileInfo.Name
            };

            FocusInfos.Add(loadInfo);
        }

        OnIgnoreConfigListChange();
    }

    void GetAllLoadInfo()
    {
        if (string.IsNullOrEmpty(ConfigPath)) return;

        if (!Directory.Exists(ConfigPath)) return;

        // SvnHelper.UpdateSvnDirectory(ConfigPath);
        LoadInfos.Clear();
        DirectoryInfo folder = new DirectoryInfo(ConfigPath);

        List<string> openList = new List<string>();

        var files = folder.GetFiles("*.xlsx");

        foreach (FileInfo file in files)
        {
            if (file.FullName.Contains("~$"))
            {
                if (files.Any(item => item.FullName == file.FullName.Replace("~$", "")))
                {
                    try
                    {
                        FileStream stream = File.Open(file.FullName.Replace("~$", ""), FileMode.Open, FileAccess.ReadWrite); //读取文件流
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream); //读取Excel*/

                        excelReader.Close();
                        stream.Close();
                        File.Delete(file.FullName);
                        continue;
                    }
                    catch { }
                }
            }

            if (openList.Contains(file.FullName.Replace("~$", ""))) continue;

            LoadInfo loadInfo = new LoadInfo
            {
                IsLoad = false,
                Path = file.FullName.Replace("~$", ""),
                Name = file.Name.Replace("~$", "")
            };

            if (file.FullName.Contains("~$"))
            {
                openList.Add(file.FullName.Replace("~$", ""));
                LoadInfos.Remove(LoadInfos.Where(item => item.Path == file.FullName.Replace("~$", "")).FirstOrDefault());
                continue;
            }

            if (FocusInfos.Contains(loadInfo)) continue;

            LoadInfos.Add(loadInfo);
        }
    }

    void SelectAll()
    {
        foreach (var item in LoadInfos)
        {
            item.IsLoad = true;
        }
    }

    void SelectOthers()
    {
        foreach (var item in LoadInfos)
        {
            item.IsLoad = !item.IsLoad;
        }
    }

    void DrawButtonGroup()
    {
        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
        {
            GetAllLoadInfo();
        }

        if (SirenixEditorGUI.ToolbarButton(EditorIcons.PenAdd))
        {
            SelectAll();
        }

        if (SirenixEditorGUI.ToolbarButton(EditorIcons.PenMinus))
        {
            SelectOthers();
        }
    }

    [Button("导入")]
    void StartLoad()
    {
        List<LoadInfo> datas = new List<LoadInfo>();
        datas.AddRange(LoadInfos);
        datas.AddRange(FocusInfos);
        ExcelLoader.Load(datas);

        // if (LoadAndCommit)
        // {
        //     SvnHelper.CommitSvnDirectory(Application.dataPath + "/Resources/i18n_default/config");
        //     SvnHelper.CommitSvnDirectory(Application.dataPath + "/Scripts/DataTable");
        // }
    }

    [System.Serializable]
    public class LoadInfo
    {
        [ToggleLeft]
        [LabelText("$Name")]
        [CustomContextMenu("加入到关注列表", nameof(AddToFocusList))]
        [CustomContextMenu("从关注列表移除", nameof(RemoveToFocusList))]
        public bool IsLoad;

        [ShowIf(nameof(FalseFun))]
        public string Path;

        [ShowIf(nameof(FalseFun))]
        public string Name;

        bool FalseFun() => false;

        void AddToFocusList()
        {
            if (!Instance.FocusInfos.Contains(this))
            {
                this.IsLoad = false;
                Instance.FocusInfos.Add(this);
            }

            Instance.GetAllLoadInfo();
            Instance.OnIgnoreConfigListChange();
        }

        void RemoveToFocusList()
        {
            if (Instance.FocusInfos.Contains(this))
            {
                Instance.FocusInfos.Remove(this);
            }

            Instance.GetAllLoadInfo();
            Instance.OnIgnoreConfigListChange();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LoadInfo)) return false;
            LoadInfo other = (LoadInfo)obj;

            return other.Path == this.Path;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }
    }
}
