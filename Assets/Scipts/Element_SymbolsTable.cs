using UnityEngine;
using TMPro;

public class Element_SymbolsTable : MonoBehaviour{

    [SerializeField] TextMeshProUGUI Text;
    SymbolsTable SymbolsTable;
    public void AddNewElement(string Data ,Transform Father, int size, string FatherList)
    {
        SymbolsTable = FindObjectOfType<SymbolsTable>();
        Text.text = Data;
        Text.fontSize = size;
        switch (FatherList)
        {
            case "Lexeme":
                SymbolsTable.ObjLexeme.Add(Instantiate(this.gameObject, Father));
                break;
            case "Token":
                SymbolsTable.ObjToken.Add(Instantiate(this.gameObject, Father));
                break;
            case "Type":
                SymbolsTable.ObjType.Add(Instantiate(this.gameObject, Father));
                break;
            case "No_Line":
                SymbolsTable.ObjNo_Line.Add(Instantiate(this.gameObject, Father));
                break;
            case "Value":
                SymbolsTable.ObjNo_Value.Add(Instantiate(this.gameObject, Father));
                break;
            case "Desplacement":
                SymbolsTable.ObjNo_Desplacement.Add(Instantiate(this.gameObject, Father));
                break;
            default:
                 Instantiate(this.gameObject, Father);
                break;
        }
    }
    

}