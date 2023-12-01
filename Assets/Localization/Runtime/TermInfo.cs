using System;
using MbsCore.Localization.Infrastructure;
using UnityEngine;

namespace MbsCore.Localization.Runtime
{
    [Serializable]
    public sealed class TermInfo : ITermInfo
    {
        [SerializeField] private string _key = string.Empty;
        [SerializeField] private string _term = string.Empty;

        public string Key => _key;
        public string Term => _term;

        public TermInfo(string key, string term)
        {
            _key = key;
            _term = term;
        }
    }
}