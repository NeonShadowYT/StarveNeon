using UnityEngine;
[System.Serializable] // ���� ����������� ��������� ���� ����� � ����
public class QuastData
{
    public string[] quastNames;
    public QuastData(QuastManager quastManager)
    {
        quastNames = new string[quastManager.AcceptQuastList.Count];
        for (int i = 0; i < quastManager.AcceptQuastList.Count; i++) quastNames[i] = quastManager.AcceptQuastList[i].name;
    }
}