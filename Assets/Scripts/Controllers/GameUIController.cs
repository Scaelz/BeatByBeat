using EventBusSystem;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameUIController : MonoBehaviour, ITrackIsOverHandler
{
    [SerializeField]
    GameObject _trackSelectionPanel;
    [SerializeField]
    GameObject _loadingPanel;
    [SerializeField]
    GameObject _endGamePanel;
    [SerializeField]
    Image _progressFill;

    UIPanel _currentPanel = UIPanel.TrackSelection;
    float _barPercent;

    public void Init(Button trackButton, AudioClip[] trackList)
    {
        EventBus.Subscribe(this);
        foreach (var track in trackList)
        {
            var button = Instantiate(trackButton, _trackSelectionPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = track.name;
            button.onClick.AddListener(() => TrackButtonClick(track));
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(this);
    }

    public void CloseSession()
    {
        ShowPanel(UIPanel.EndGame);
    }

    public void EndGameButtonClick(int option)
    {
        if (option == 0)
        {
            ShowPanel(UIPanel.None);
        }
        EventBus.RaiseEvent<IApplicationRequestHandler>(
            x => x.ApplycationRequestHandle((ApplicationRequest)option));
    }

    void TrackButtonClick(AudioClip clip)
    {
        ShowPanel(UIPanel.Loading);
        EventBus.RaiseEvent<ITrackSelectedHandler>(x => x.PrepareTrack(clip, UpdateProgressBar));
    }

    void UpdateProgressBar(float percent)
    {
        _barPercent = percent * 0.01f;
    }

    private void Update()
    {
        if (_progressFill.fillAmount != _barPercent)
        {
            _progressFill.fillAmount = _barPercent;
        }
        if (_barPercent == 1)
        {
            StartCoroutine(DelayedHideLoadPanel());
        }
    }

    IEnumerator DelayedHideLoadPanel()
    {
        _barPercent = 0;
        yield return null;
        ShowPanel(UIPanel.None);
        yield return new WaitForSeconds(2);
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
                return _endGamePanel;
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
        _currentPanel = uiPanel;
        if (panelGO == null)
        {
            return;
        }
        panelGO.transform.DOLocalMoveX(0, 0.75f).SetEase(Ease.OutElastic);
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
