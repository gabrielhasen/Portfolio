using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    public Vector3 offset;
    public float dragSpeed;
    public LeanTweenType easeType;
    
    private bool canMove;
    private float zPosition;

    private void Start() {
        canMove = false;
        zPosition = transform.position.z;
    }

    private void Update() {
        if (!canMove) return;
        MoveObject();
    }

    private void MoveObject() {
        Vector3 pos = Input.mousePosition + offset;
        pos.z = zPosition;
        LeanTween.move(this.gameObject, pos, dragSpeed/4).setEase(easeType); // MoveUpdate(this.gameObject, pos, dragSpeed);
    }

    public void OnPointerDown(PointerEventData eventData) {
        canMove = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        canMove = false;
    }
}
