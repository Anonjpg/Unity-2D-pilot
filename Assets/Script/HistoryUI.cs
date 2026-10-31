using UnityEngine;
using TMPro;

public class HistoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject historyPanel;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text contentText;

    public bool IsOpen { get; private set; }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            historyPanel.SetActive(false);
            IsOpen = false;
        }
    }

    public void ShowHistory(HistoricalFragment fragment)
    {
        dateText.text = fragment.historicalDate;
        contentText.text = fragment.content;

        historyPanel.SetActive(true);
        IsOpen = true;
    }
}
