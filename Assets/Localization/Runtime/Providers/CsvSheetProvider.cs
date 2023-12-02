using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MbsCore.Localization.Runtime.Providers
{
    [CreateAssetMenu(menuName = "Localization/Sheet Provider/Csv Provider",
                     fileName = nameof(CsvSheetProvider),
                     order = 51)]
    internal sealed class CsvSheetProvider : SheetProvider
    {
        [SerializeField] private EncodingType _encodingType = EncodingType.Default;
        [SerializeField] private string _fileName = string.Empty;
        
        public override Dictionary<string, List<string>> LoadSheet()
        {
            var csvData = new Dictionary<string, List<string>>();
            if (!string.IsNullOrEmpty(_fileName))
            {
                using var reader = new StreamReader(_fileName, EncodingTools.GetEncoding(_encodingType));
                string line = reader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] columnHeaders = line.Split(',');
                    foreach (var header in columnHeaders)
                    {
                        csvData[header.Trim()] = new List<string>();
                    }

                    while (!reader.EndOfStream)
                    {
                        string dataLine = reader.ReadLine();
                        if (string.IsNullOrEmpty(dataLine))
                        {
                            continue;
                        }
                        
                        string[] data = dataLine.Split(',');
                        for (int i = 0; i < columnHeaders.Length; i++)
                        {
                            csvData[columnHeaders[i].Trim()].Add(data[i].Trim());
                        }
                    }   
                }
            }

            return csvData;
        }
    }
}