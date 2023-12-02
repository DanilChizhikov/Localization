using System.Collections.Generic;
using MbsCore.Localization.Infrastructure;
using UnityEngine;

namespace MbsCore.Localization.Runtime
{
    public abstract class SheetProvider : ScriptableObject, ISheetProvider
    {
        public abstract Dictionary<string, List<string>> LoadSheet();
    }
}