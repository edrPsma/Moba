using System.Collections.Generic;

namespace UnityCodeGen
{
    public sealed class GeneratorContext
    {
        private List<CodeText> _codeList = new List<CodeText>();
        internal IReadOnlyList<CodeText> codeList => _codeList;

        public void AddCode(string fileName, string text)
        {
            _codeList.Add(new CodeText() { path = UnityCodeGenUtility.defaultFolderPath, fileName = fileName, text = text });
        }

        public void AddCode(string path, string fileName, string text)
        {
            _codeList.Add(new CodeText() { path = path, fileName = fileName, text = text });
        }
    }
}