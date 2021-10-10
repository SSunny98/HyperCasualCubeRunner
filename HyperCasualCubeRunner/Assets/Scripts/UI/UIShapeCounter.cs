using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShapeCounter : MonoBehaviour
{
    [SerializeField] private List<Image> allSections = new List<Image>();
    [SerializeField] private Sprite sectionEmpty = null;
    [SerializeField] private Sprite sectionFilled = null;

    public void UpdateSectionCounter(int currentCounter)
    {
        for (int i = 0; i < allSections.Count; i++)
        {
            if (i < currentCounter)
                allSections[i].sprite = sectionFilled;
            else
                allSections[i].sprite = sectionEmpty;
        }
    }
}
