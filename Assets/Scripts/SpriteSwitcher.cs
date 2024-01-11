using System;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite zoomedInSprite;
    public Sprite zoomedOutSprite;
    public Camera mainCamera;
    public CircleCollider2D collider2D;

    public float zoomThreshold = 5f;
    public float zoomedOutScaleFactor = 0.2f;
    public float zoomedInScaleFactor = 0.2f;
    public float zoomedInRadius = 1.62f;
    public float zoomedOutRadius = 5.5f;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (mainCamera.orthographicSize <= zoomThreshold)
        {
            spriteRenderer.sprite = zoomedInSprite;
            spriteRenderer.transform.localScale = Vector3.one * zoomedInScaleFactor;
            collider2D.radius = zoomedInRadius;
        }
        else
        {
            spriteRenderer.sprite = zoomedOutSprite;
            spriteRenderer.transform.localScale = Vector3.one * zoomedOutScaleFactor;
            collider2D.radius = zoomedOutRadius;
        }
    }
}