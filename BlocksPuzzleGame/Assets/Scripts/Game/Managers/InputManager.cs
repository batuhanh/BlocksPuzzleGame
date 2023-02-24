using Game.Core.GridSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        private bool canClick = false;
        BlockObject hittedBlock = null;
        Vector3 moveOffset;
        private int clickCount=0;
        void Update()
        {
            if (canClick)
            {

#if UNITY_EDITOR
                GetEditorInputs();
#else
		        GetMobileTouches();
#endif

            }
        }
        private void GetEditorInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {

                hittedBlock = DetectHittedObject(Input.mousePosition);
                if (hittedBlock != null )
                {
                    clickCount++;
                    moveOffset = hittedBlock.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    hittedBlock.OnHolded(clickCount);
                }
            }
            if (hittedBlock != null) 
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;
                moveOffset.z = 0;
                hittedBlock.transform.position = worldPos + moveOffset;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (hittedBlock != null)
                {
                    hittedBlock.OnReleased();
                }
                hittedBlock = null;
            }
        }
        private void GetMobileTouches()
        {
            Touch touch = Input.GetTouch(0);   
            if (touch.phase.Equals(TouchPhase.Began))
            {
                hittedBlock = DetectHittedObject(touch.position);
                if (hittedBlock != null)
                {
                    clickCount++;
                    moveOffset = hittedBlock.transform.position - Camera.main.ScreenToWorldPoint(touch.position);
                    hittedBlock.OnHolded(clickCount);
                }
            }
            if (hittedBlock != null)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touch.position);
                worldPos.z = 0;
                moveOffset.z = 0;
                hittedBlock.transform.position = worldPos + moveOffset;
            }
            if (touch.phase.Equals(TouchPhase.Ended))
            {
                if (hittedBlock != null)
                {
                    hittedBlock.OnReleased();
                }
                hittedBlock = null;
            }
        }
        private BlockObject DetectHittedObject(Vector3 touchedPos)
        {
            //Check if game active
            PolygonCollider2D hittedCollider = Physics2D.OverlapPoint(mainCam.ScreenToWorldPoint(touchedPos)) as PolygonCollider2D;
            if (hittedCollider)
            {
                GameObject hittedObj = hittedCollider.gameObject;
                if (hittedObj.CompareTag("BlockPart"))
                {
                    return hittedObj.GetComponentInParent<BlockObject>();   
                }
            }
            return null;
        }
        private void EnableClicking()
        {
            canClick = true;
        }
        private void DisableClicking()
        {
            canClick = false;
        }
        private void OnEnable()
        {
            LevelManager.levelStartedEvent += EnableClicking;
            LevelManager.levelSuccesedEvent += DisableClicking;
        }
        private void OnDisable()
        {
            LevelManager.levelStartedEvent -= EnableClicking;
            LevelManager.levelSuccesedEvent -= DisableClicking;
        }
    }
}
