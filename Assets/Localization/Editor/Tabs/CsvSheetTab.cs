using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MbsCore.Localization.Infrastructure;
using MbsCore.Localization.Runtime;
using UnityEditor;
using UnityEngine;

namespace MbsCore.Localization.Editor.Tabs
{
    internal sealed class CsvSheetTab : ISheetTab
    {
        private const string DialogMessage = "Importing from CSV will overwrite any existing languages or fields with" +
                                             "the same name in the current contents. Are you sure?";
        private const string ValuePattern = "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)";
        
        public string TabName => "Csv";

        private EncodingType _encodingType;
        private string _csvFilename;

        public CsvSheetTab()
        {
            _encodingType = EncodingType.Default;
            _csvFilename = string.Empty;
        }
        
        public void DrawGui(SerializedProperty groupProperty)
        {
            _encodingType = (EncodingType) EditorGUILayout.EnumPopup("Encoding", _encodingType);
            EditorHelper.ButtonDraw("Import", () =>  DrawImportCsvDialogs(groupProperty));
        }

        private void DrawImportCsvDialogs(SerializedProperty groupProperty)
        {
            if (!EditorUtility.DisplayDialog("Import CSV?", DialogMessage, "Import", "Cancel"))
            {
                return;
            }

            string fileName = EditorUtility.OpenFilePanel("Import from CSV",
                                                          GetPath(_csvFilename),"csv");
            if (string.IsNullOrEmpty(fileName) ||
                !File.Exists(fileName))
            {
                EditorUtility.DisplayDialog("Import CSV", "Can't find the file " + fileName + ".", "OK");
                return;
            }

            _csvFilename = fileName;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                _csvFilename = _csvFilename.Replace("/", "\\");
            }

            bool result = TryImportCsv(_csvFilename, groupProperty);
            if (result == false)
            {
                EditorUtility.DisplayDialog("Import CSV", "Can't open the file " + fileName + ".", "OK");
            }
        }
        
        private bool TryImportCsv(string csvFile, SerializedProperty groupProperty)
        {
            if (!TryReadCsvFile(csvFile, _encodingType, out List<List<object>> content) ||
                content.Count == 0 ||
                content[0].Count < 2)
            {
                return false;
            }

            LanguageGroup[] groups = ConvertToGroups(content);
            groupProperty.boxedValue = groups;
            for (int i = groups.Length - 1; i >= 0; i--)
            {
                SerializedProperty property = groupProperty.GetArrayElementAtIndex(i);
                property.boxedValue = groups[i];
            }
            
            return true;
        }
        
        private string GetPath(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return string.Empty;
            }

            return Path.GetDirectoryName(filename);
        }
        
        private bool TryReadCsvFile(string filename, EncodingType encodingType, out List<List<object>> result)
        {
            result = new List<List<object>>();
            List<string> sourceList = new List<string>();
            using (var file = new StreamReader(filename, EncodingTools.GetEncoding(encodingType)))
            {
                string line = string.Empty;
                while (!string.IsNullOrEmpty(line = file.ReadLine()))
                {
                    sourceList.Add(line.TrimEnd());
                }
            }

            CombineMultilineSourceLines(sourceList);
            if (sourceList.Count < 1)
            {
                return false;
            }

            const int ZeroIndex = 0;
            while (sourceList.Count > ZeroIndex)
            {
                string[] values = GetValues(sourceList[ZeroIndex]);
                sourceList.RemoveAt(ZeroIndex);
                if (ReferenceEquals(values, null) ||
                    values.Length == 0)
                {
                    continue;
                }

                var row = new List<object>(values);
                result.Add(row);
            }

            return true;
        }
        
        private void CombineMultilineSourceLines(List<string> sourceLines)
        {
            const int MaxIterations = 999999;
            
            int lineNum = 0;
            int safeguard = 0;
            while ((lineNum < sourceLines.Count) && (safeguard < MaxIterations))
            {
                safeguard++;
                string line = sourceLines[lineNum];
                if (line == null)
                {
                    sourceLines.RemoveAt(lineNum);
                }
                else
                {
                    bool terminated = true;
                    char previousChar = (char)0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        char currentChar = line[i];
                        bool isQuote = (currentChar == '"') && (previousChar != '\\');
                        if (isQuote) terminated = !terminated;
                        previousChar = currentChar;
                    }
                    if (terminated || (lineNum + 1) >= sourceLines.Count)
                    {
                        if (!terminated) sourceLines[lineNum] = line + '"';
                        lineNum++;
                    }
                    else
                    {
                        sourceLines[lineNum] = line + "\\n" + sourceLines[lineNum + 1];
                        sourceLines.RemoveAt(lineNum + 1);
                    }
                }
            }
        }
        
        private string[] GetValues(string line)
        {
            var csvSplit = new Regex(ValuePattern);
            var values = new List<string>();
            foreach (Match match in csvSplit.Matches(line))
            {
                values.Add(UnwrapValue(match.Value.TrimStart(',')));
            }
            
            return values.ToArray();
        }
        
        private string UnwrapValue(string value)
        {
            string s = value.Replace("\\n", "\n").Replace("\\r", "\r");
            if (s.StartsWith("\"") && s.EndsWith("\""))
            {
                s = s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
            }
            
            return s;
        }
        
        private LanguageGroup[] ConvertToGroups(List<List<object>> content)
        {
            var groups = new List<LanguageGroup>();
            List<object> headers = content[0];
            content.RemoveAt(0);
            for (int i = 1; i < headers.Count; i++)
            {
                var language = Enum.Parse<SystemLanguage>(headers[i].ToString().Trim(), true);
                var terms = new List<TermInfo>();
                for (int j = 0; j < content.Count; j++)
                {
                    List<object> data = content[j];
                    terms.Add(new TermInfo(data[0].ToString().Trim(), data[i].ToString().Trim()));
                }
                
                groups.Add(new LanguageGroup(language, terms.ToArray()));
            }
            
            return groups.ToArray();
        }
    }
}