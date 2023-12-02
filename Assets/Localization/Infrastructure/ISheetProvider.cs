using System.Collections.Generic;

namespace MbsCore.Localization.Infrastructure
{
    public interface ISheetProvider
    {
        Dictionary<string, List<string>> LoadSheet();
    }
}