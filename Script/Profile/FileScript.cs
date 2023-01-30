using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FileScript : MonoBehaviour
{
    public TMP_Text fileNameText;
    [HideInInspector]
    public int index;
    public void OnClick() => AvatarManager.instance.SelectAvatar(index);
}
