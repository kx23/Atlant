using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    //need change

    private float _activeTime = 0.5f;
    private float _timeActivated;
    private float _alpha;
    private float _alphaSet=0.9f;
    private float _alphaMultiplier=0.99f;

    [SerializeField]
    private Transform _player;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private SpriteRenderer _playerSpriteRenderer;

    private Color _color;

    /*private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = transform.parent.transform;
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }*/
    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerSpriteRenderer = _player.Find("Body").GetComponent<SpriteRenderer>();
        _alpha = _alphaSet;
        _spriteRenderer.sprite = _playerSpriteRenderer.sprite;

        transform.position = _player.position;
        transform.rotation = _player.rotation;
        _timeActivated = Time.time;
    }

    private void Update()
    {
        _alpha *= _alphaMultiplier;
        _color = new Color(1f, 1f, 1f, _alpha);
        _spriteRenderer.color = _color;

        if (Time.time >= (_timeActivated + _activeTime))
        {
            PlayerAfterImagePool.instance.AddToPool(gameObject);
        }
    }

}
