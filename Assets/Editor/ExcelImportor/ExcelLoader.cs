using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using UnityEditor;
using UnityEngine;
using static LoadConfigWnd;

public static class ExcelLoader
{
    const string ConfigPath = "GameAssets/Config";
    const string ScriptPath = "HotUpdate/DataTable/Generate";
    static IgnoreConfig _ignoreConfig;
    public static void Load(List<LoadInfo> loadInfos)
    {
        _ignoreConfig = Resources.Load<IgnoreConfig>("IgnoreConfig");
        FieldMapper fieldMapper = Resources.Load<FieldMapper>("FieldMapper");
        List<ExcelInfo> list = new List<ExcelInfo>();
        for (int i = 0; i < loadInfos.Count; i++)
        {
            if (!loadInfos[i].IsLoad) continue;
            EditorUtility.DisplayProgressBar("配置表导入", $"加载配置表: {loadInfos[i].Name}", i * 1f / loadInfos.Count);

            ExcelInfo excelInfo = ReadExcel(loadInfos[i].Path, fieldMapper);
            if (excelInfo == null) continue;

            list.Add(excelInfo);
            CreateScript(excelInfo);
            // CreateCSV(excelInfo);
            CreateJSON(excelInfo);
        }

        Debug.Log("导入完成");
        // int trunkIndex = Application.dataPath.IndexOf("trunk");
        // string serverToolPath = Application.dataPath.Substring(0, trunkIndex + 5) + "/Server/Tools/导表.bat";
        // string workPath = Application.dataPath.Substring(0, trunkIndex + 5) + "/Server/Tools";
        // BatExcutor.LaunchBat(serverToolPath, "", workPath);

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static ExcelInfo ReadExcel(string path, FieldMapper fieldMapper)
    {
        System.Data.DataSet dataSet = null;
        try
        {
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite); //读取文件流
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream); //读取Excel*/
            dataSet = excelReader.AsDataSet();
        }
        catch
        {
            Debug.LogError(string.Format("导表失败, path:{0}", path));
            return null;
        }

        System.Data.DataTable table = null;
        foreach (System.Data.DataTable item in dataSet.Tables)
        {
            if (item.ToString().ToLower().Contains("c"))
            {
                table = item;
                break;
            }
        }

        if (table == null) return null;
        if (table.Rows.Count < 4) return null;

        ExcelInfo excelInfo = new ExcelInfo();

        int endIndex = path.LastIndexOf('.');
        int startIndex = path.LastIndexOf('\\');

        excelInfo.Name = path.Substring(startIndex + 1, endIndex - startIndex - 1);
        excelInfo.DataTable = table;
        for (int j = 0; j < table.Columns.Count; j++)
        {
            if (string.IsNullOrEmpty(table.Rows[3][j].ToString())) continue;

            string platform = table.Rows[1][j].ToString();

            if (!string.IsNullOrEmpty(platform) && !platform.Contains("c") && !platform.Contains("#")) continue;

            excelInfo.Columns.Add(j);
            excelInfo.Types.Add(fieldMapper.Map(ExcelLoaderUtils.Format(table.Rows[3][j].ToString())));
            excelInfo.Names.Add(ExcelLoaderUtils.Format(table.Rows[2][j].ToString()));
            excelInfo.Descs.Add(ExcelLoaderUtils.Format(table.Rows[0][j].ToString()));
        }

        return excelInfo.Columns == null ? null : excelInfo;
    }

    static void CreateScript(ExcelInfo excelInfo)
    {
        string scriptName = ExcelLoaderUtils.BigHump(excelInfo.Name);
        if (_ignoreConfig.ScriptIgnoreList.Contains(scriptName)) return;

        var folderPath = $"{Application.dataPath}/{ScriptPath}";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        var codePath = $"{folderPath}/DT{scriptName}.cs";

        Debug.Log(codePath);

        FileStream fs = File.Open(codePath, FileMode.Create, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(NamespaceGenerator.Generate());
        sw.WriteLine();
        sw.Write(ClassNameGenerator.Generate(scriptName));
        sw.WriteLine("{");
        sw.Write(CustomGenerator.Generate(scriptName, excelInfo));
        for (int i = 0; i < excelInfo.Types.Count; i++)
        {
            IFieldGenerator fieldGenerator = FieldGenerator.GetFieldGenerator(excelInfo.Types[i]);
            string context = fieldGenerator.Generate(excelInfo.Types[i], excelInfo.Names[i], excelInfo.Descs[i]);
            sw.WriteLine(context);
        }
        sw.WriteLine("}");
        sw.Close();
        fs.Close();
    }

    static void CreateCSV(ExcelInfo excelInfo)
    {
        string csvName = ExcelLoaderUtils.BigHump(excelInfo.Name);
        if (_ignoreConfig.CSVIgnoreList.Contains(csvName)) return;

        var folderPath = $"{Application.dataPath}/{ConfigPath}";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        var csvPath = $"{folderPath}/{csvName}.csv";
        using (FileStream fs = File.Open(csvPath, File.Exists(csvPath) ? FileMode.Truncate : FileMode.OpenOrCreate, FileAccess.ReadWrite))//读取文件流
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < excelInfo.Names.Count; i++)
            {
                builder.Append(excelInfo.Names[i]);
                if (i != excelInfo.Names.Count - 1)
                {
                    builder.Append("^");
                }
                else
                {
                    builder.Append("\r\n");
                }
            }

            for (int i = 4; i < excelInfo.DataTable.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(excelInfo.DataTable.Rows[i][0].ToString())) continue;

                for (int j = 0; j < excelInfo.Columns.Count; j++)
                {
                    string name = excelInfo.Names[j];
                    string type = excelInfo.Types[j];
                    string value = ExcelLoaderUtils.CheckValue(excelInfo.DataTable.Rows[i][excelInfo.Columns[j]].ToString(), type);
                    builder.Append(value);
                    if (j == excelInfo.Columns.Count - 1)
                    {
                        builder.Append("\r\n");
                    }
                    else
                    {
                        builder.Append("^");
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(builder.ToString());
            }
        }
    }

    static void CreateJSON(ExcelInfo excelInfo)
    {
        string jsonName = ExcelLoaderUtils.BigHump(excelInfo.Name);
        if (_ignoreConfig.CSVIgnoreList.Contains(jsonName)) return;

        var folderPath = $"{Application.dataPath}/{ConfigPath}";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        var jsonPath = $"{folderPath}/DT{jsonName}.json";
        using (FileStream fs = File.Open(jsonPath, File.Exists(jsonPath) ? FileMode.Truncate : FileMode.OpenOrCreate, FileAccess.ReadWrite))//读取文件流
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("{\r\n");

            for (int i = 4; i < excelInfo.DataTable.Rows.Count; i++)
            {
                string id = excelInfo.DataTable.Rows[i][0].ToString();
                if (string.IsNullOrEmpty(id)) continue;

                StringBuilder content = new StringBuilder();
                content.Append("{");
                for (int j = 0; j < excelInfo.Columns.Count; j++)
                {
                    string name = excelInfo.Names[j];
                    string type = excelInfo.Types[j];
                    string tempValue = excelInfo.DataTable.Rows[i][excelInfo.Columns[j]].ToString();
                    string value = ExcelLoaderUtils.CheckValue(tempValue, type);


                    if (ExcelLoaderUtils.IsNumeric(value) && type != "string")
                    {
                        content.Append($"\"{name}\":{value}");
                    }
                    else if (ExcelLoaderUtils.IsArray(type))
                    {
                        content.Append($"\"{name}\":{value}");
                    }
                    else
                    {
                        content.Append($"\"{name}\":\"{value}\"");
                    }
                    if (j != excelInfo.Columns.Count - 1)
                    {
                        content.Append(",");
                    }
                }
                content.Append("}");
                if (i == excelInfo.DataTable.Rows.Count - 1)
                {
                    builder.Append($"\t\"{id}\":{content}\r\n");
                }
                else
                {
                    builder.Append($"\t\"{id}\":{content},\r\n");
                }
            }

            builder.Append("\r\n}");

            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(builder.ToString());
            }
        }
    }
}

public class ExcelInfo
{
    public string Name;
    public System.Data.DataTable DataTable;
    public List<int> Columns = new List<int>();
    public List<string> Types = new List<string>();
    public List<string> Names = new List<string>();
    public List<string> Descs = new List<string>();
}