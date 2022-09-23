using System.Collections.Generic;
using UnityEngine;

public class Console_Errors : MonoBehaviour{


    [Header("-----Text-----")]
    [SerializeField] List<string> No_Line;
    [SerializeField] List<string> Description;

    [Header("-----AddElements-----")]
    [SerializeField] ConsoleElements ConsoleElements;
    [SerializeField] GameObject FatherLine;
    [SerializeField] GameObject FatherDescription;
    [SerializeField] RectTransform ConsoleTransform;

    [Header("-----DeleteElements-----")]
    [SerializeField] public List<GameObject> ObjLine;
    [SerializeField] public List<GameObject> ObjDescription;


    public void DebugConsole(int Line, string Log)
    {
        ConsoleTransform.sizeDelta = new Vector2(ConsoleTransform.sizeDelta.x, ConsoleTransform.sizeDelta.y + 37);

        No_Line.Add(Line.ToString());
        Description.Add(Log);
        ConsoleElements.AddNewElement(Log, FatherDescription.transform, 30, "Log");
        ConsoleElements.AddNewElement(Line.ToString(), FatherLine.transform, 30, "Line");

    }

    [ContextMenu("Clean")]
    public void Console_Clear()
    {
        No_Line = new List<string>();
        Description = new List<string>();

        GameObject[] newlog = ObjDescription.ToArray();
        GameObject[] newline = ObjLine.ToArray();

        for (int i = 0; i < newlog.Length; i++)
        {
            Destroy(newlog[i]);
            Destroy(newline[i]);
            ConsoleTransform.sizeDelta = new Vector2(ConsoleTransform.sizeDelta.x, ConsoleTransform.sizeDelta.y - 37);

        }

        ObjDescription = new List<GameObject>();
        ObjLine = new List<GameObject>();
    }

}