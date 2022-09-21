using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleElements : MonoBehaviour{

    [SerializeField] TextMeshProUGUI Text;
    Console_Errors Console_Errors;
    public void AddNewElement(string Data, Transform Father, int size, string FatherList)
    {
        Console_Errors = FindObjectOfType<Console_Errors>();
        Text.text = Data;
        Text.fontSize = size;
        switch (FatherList)
        {
            case "Log":
                Console_Errors.ObjDescription.Add(Instantiate(this.gameObject, Father));
                break;
            case "Line":
                Console_Errors.ObjLine.Add(Instantiate(this.gameObject, Father));
                break;
        }
    }

}