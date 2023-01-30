using UnityEngine;

public class ModIOOpener : MonoBehaviour
{
    public void Open()
    {
        ModIOBrowser.Browser.OpenBrowser(null);
    }
}
