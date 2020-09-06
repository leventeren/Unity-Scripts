Collectible.cs

using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public event Action<Collectible> OnPickup;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            OnPickup?.Invoke(this);

            gameObject.SetActive(false);
        }
    }
}

Collector.cs

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] List<Collectible> _gatherables;
    
    [SerializeField] UnityEvent OnCompleteEvent;

    List<Collectible> _collectiblesRemaining;
    

    void OnEnable()
    {
        _collectiblesRemaining = new List<Collectible>(_gatherables);

        foreach (var collectible in _collectiblesRemaining)
            collectible.OnPickup += HandlePickup; // Registering for the OnPickup event on Collectible
        
        UpdateText();
    }

    void HandlePickup(Collectible collectible)
    {
        _collectiblesRemaining.Remove(collectible);
        UpdateText();
        
        if (_collectiblesRemaining.Count == 0)
            OnCompleteEvent.Invoke();
    }

    void UpdateText()
    {
        _text.SetText($"{_collectiblesRemaining.Count} more...");
        if (_collectiblesRemaining.Count == 0 || _collectiblesRemaining.Count == _gatherables.Count)
            _text.enabled = false;
        else
            _text.enabled = true;
    }

    [ContextMenu("AutoFill Collectibles")]
    void AutoFillCollectibles()
    {
        _gatherables = GetComponentsInChildren<Collectible>()
            .Where(t=> t.name.ToLower().Contains("red"))
            .ToList();
    }
}
