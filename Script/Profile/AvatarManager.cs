using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
public class AvatarManager : MonoBehaviour
{
    public GameObject filesListPan, filesContent, filePrefab, profilePanel;
    public RawImage avatarImg;
    private DirectoryInfo dirInfo = new DirectoryInfo("Starve Neon Avatar/Avatar");
    private FileInfo[] files;
    private GameObject[] instancedObjs;
    public static AvatarManager instance;
    private void Awake() => instance = this;
    public void LoadAvatarsList()
    {
        filesListPan.SetActive(true); avatarImg.gameObject.SetActive(false);
        files = new string[] { "*.png", "*.jpeg", "*.jpg" }.SelectMany(ext => dirInfo.GetFiles(ext, SearchOption.AllDirectories)).ToArray();
        instancedObjs = new GameObject[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            FileScript file = Instantiate(filePrefab, filesContent.transform).GetComponent<FileScript>();
            file.fileNameText.text = files[i].Name;
            file.index = i;
            instancedObjs[i] = file.gameObject;
        }
    }
    public void SelectAvatar(int index)
    {
        WWW www = new WWW("file://" + files[index].FullName);
        avatarImg.texture = www.texture;
        filesListPan.SetActive(false); avatarImg.gameObject.SetActive(true); profilePanel.gameObject.SetActive(true);
        foreach (GameObject obj in instancedObjs) Destroy(obj);
    }
}
