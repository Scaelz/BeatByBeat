using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using EventBusSystem;
using DG.Tweening;

public enum UIPanel
{
    TrackSelection,
    Loading,
    EndGame,
    None
}

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    GameObject _trackSelectionPanel;
    [SerializeField]
    GameObject _loadingPanel;
    UIPanel _currentPanel = UIPanel.TrackSelection;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowPanel(UIPanel.TrackSelection);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowPanel(UIPanel.Loading);
        }
    }

    public void NotePressed(NoteButton noteButton)
    {
        EventBus.RaiseEvent<IUserInputHandler>(x => x.UpdatePlayerDestination(noteButton.note));
    }

    private GameObject PickPanelGO(UIPanel uiPanel)
    {
        switch (uiPanel)
        {
            case UIPanel.TrackSelection:
                return _trackSelectionPanel;
            case UIPanel.Loading:
                return _loadingPanel;
            case UIPanel.EndGame:
                return _loadingPanel;
            default:
                return null;
        }
    }
    public void ShowPanel(UIPanel uiPanel)
    {
        if (_currentPanel == uiPanel)
        {
            return;
        }
        GameObject panelGO = PickPanelGO(uiPanel);
        HideCurrentPanel();
        panelGO.transform.DOLocalMoveX(0, 0.75f).SetEase(Ease.OutElastic);
        _currentPanel = uiPanel;
    }

    public void HidePanel(UIPanel uiPanel)
    {
        GameObject panelGO = PickPanelGO(uiPanel);
        panelGO.transform.DOLocalMoveX(-1300, 0.15f).SetEase(Ease.InFlash);
    }

    public void HideCurrentPanel()
    {
        if (_currentPanel != UIPanel.None)
        {
            HidePanel(_currentPanel);
        }
    }
}
