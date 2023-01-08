using System;
using System.Collections.Generic;

using UnityEngine;

public class ListCont : MonoBehaviour
{

    public GameObject SlotTemplate;
    private GameObject UpButton;
    private GameObject DownButton;
    private readonly int MaxSlosts = 5;
    private int SlotIndex = 0;

    private readonly List<ListSlotBehaviour> SlotBehaviours = new List<ListSlotBehaviour>();
    private List<System.Object> listObjects = new List<System.Object>();

    private void Awake()
    {
        UpButton = transform.Find("UpButton").gameObject;
        DownButton = transform.Find("DownButton").gameObject;
    }




    void Start()
    {
        CheckMoveButtonVisibillity();
        CustomStart();
    }

    virtual public void CustomStart()
    {

    }


    private void CheckMoveButtonVisibillity()
    {
        if (listObjects?.Count > SlotIndex + MaxSlosts)
        {
            UpButton.SetActive(true);
        }
        else
        {
            UpButton.SetActive(false);
        }
        if (SlotIndex > 0)
        {
            DownButton.SetActive(true);
        }
        else
        {
            DownButton.SetActive(false);
        }
    }

    public void MoveUp()
    {
        SlotIndex += MaxSlosts;
        UpdateSlots();
        CheckMoveButtonVisibillity();
    }

    public void MoveDown()
    {
        SlotIndex -= MaxSlosts;
        UpdateSlots();
        CheckMoveButtonVisibillity();
    }


    private void UpdateSlots()
    {
        ClearSlots();
        if (this.listObjects?.Count > 0)
        {
            
            for (int i = SlotIndex; i < this.listObjects.Count; i++)
            {
                CreateAndFillSlot(i, this.listObjects[i]);
            }
        }
    }


    private void ClearSlots()
    {
        for (int i = SlotBehaviours.Count - 1; i >= 0; i--)
        {
            GameObject slot = SlotBehaviours[i].gameObject;
            GameObject.Destroy(slot);
            SlotBehaviours.RemoveAt(i);
        }
    }

    public void SetObjects(List<System.Object> listObjects)
    {
        this.listObjects = listObjects;
        SlotIndex = 0;
        UpdateSlots();
        CheckMoveButtonVisibillity();
    }

    private void CreateAndFillSlot(int index, System.Object gameState)
    {

        GameObject modeSlot = Instantiate(SlotTemplate, new Vector3(0, 0, 0), Quaternion.identity, SlotTemplate.transform.parent);
        float relative = 1f / MaxSlosts;
        RectTransform rect = modeSlot.GetComponent<RectTransform>();
        rect.anchoredPosition3D = new Vector3(0, 0, 0);
        rect.anchorMin = new Vector2(rect.anchorMin.x, (float)index * relative);
        rect.anchorMax = new Vector2(rect.anchorMax.x, (float)(index + 1) * relative);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);

        modeSlot.SetActive(true);
        modeSlot.name = "Slot " + index;


        ListSlotBehaviour slotBehaviour = modeSlot.GetComponent<ListSlotBehaviour>();
        slotBehaviour.index = index;
        this.SlotBehaviours.Add(slotBehaviour);
        slotBehaviour.RudeAwake();
        slotBehaviour.SetObject(gameState);

    }
}
