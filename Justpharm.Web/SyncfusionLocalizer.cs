namespace Justpharm.Web;

using Syncfusion.Blazor;

public class SyncfusionLocalizer : ISyncfusionStringLocalizer
{
    public string GetText(string key)
    {
        return this.ResourceManager.GetString(key)!;
    }

    public System.Resources.ResourceManager ResourceManager
    {
        get
        {
            return Resources.SfResources.ResourceManager;
        }
    }
}