using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SymbolsTable : MonoBehaviour{

    #region Variables

    [Header("-----SymbolsTable-----")]
    [SerializeField] List<string> Lexeme;
    [SerializeField] List<string> Token;
    [SerializeField] List<string> Type;
    [SerializeField] List<string> No_Line;

    [Space(20)]
    [Header("-----TokenList-----")]
    [SerializeField] List<string> TL_Lexeme;
    [SerializeField] List<string> TL_Token;
    
    [Space(20)]
    [Header("-----AddElements-----")]
    [SerializeField] Element_SymbolsTable AddElement;
    [SerializeField] GameObject FatherLexeme;
    [SerializeField] GameObject FatherToken;
    [SerializeField] GameObject FatherType;
    [SerializeField] GameObject FatherLine;

    [Space(20)]
    [Header("-----DeleteElements-----")]
    [SerializeField] public List<GameObject> ObjLexeme;
    [SerializeField] public List<GameObject> ObjToken;
    [SerializeField] public List<GameObject> ObjType;
    [SerializeField] public List<GameObject> ObjNo_Line;

    #endregion


    private void Start()
    {
        Debug.Log("hola");
        //Prellenado de la Tabla de simbolos
        string[] newLexeme = Lexeme.ToArray();
        string[] newToken = Token.ToArray();
        string[] newType = Type.ToArray();
        for (int i = 0; i < newLexeme.Length; i++)
        {
            AddElement.AddNewElement(newLexeme[i], FatherLexeme.transform, 30, "");
            AddElement.AddNewElement(newToken[i], FatherToken.transform, 30, "");
            if (newType[i] == "reservada")
            {
                AddElement.AddNewElement(newType[i], FatherType.transform,28, "");
            }
            else
            { 
                AddElement.AddNewElement(newType[i], FatherType.transform, 30, "");
            }
            AddElement.AddNewElement("", FatherLine.transform, 30, "");
        }
    }


    void Add_new_Lexeme(string NewLexeme, string NewToken, int Num_Line)
    {
        //llenado de la Tabla de simbolos
        Lexeme.Add(NewLexeme);
        AddElement.AddNewElement(NewLexeme, FatherLexeme.transform, 30,"Lexeme");
        Token.Add(NewToken);
        if (NewToken == "Identificador")
        {
            AddElement.AddNewElement(NewToken, FatherToken.transform, 24, "Token");
        }
        else
        {
            AddElement.AddNewElement(NewToken, FatherToken.transform, 30,"Token");
        }

        
        switch (NewLexeme)
        {
            case "int":
                Type.Add(NewLexeme);
                AddElement.AddNewElement(NewLexeme,FatherType.transform, 30, "Type");
                break;
            case "float":
                Type.Add(NewLexeme);
                AddElement.AddNewElement(NewLexeme, FatherType.transform, 36, "Type");
                break;
            default:
                AddElement.AddNewElement("", FatherType.transform, 30, "Type");
                break;
        }


        No_Line.Add(Num_Line.ToString());
        AddElement.AddNewElement(Num_Line.ToString(), FatherLine.transform, 30, "No_Line");


    }


    public void Add_New_Token(string NewLexeme, string NewToken, int Num_Line)
    {
        //Añadido de los elementos a la lista de Tokens
        TL_Lexeme.Add(NewLexeme);
        TL_Token.Add(NewToken);

        //Revision de los tokes
        NewToken = Update_Tokens(NewLexeme);
        //Añadido de los elementos a la tabla de simbolos
        Add_new_Lexeme(NewLexeme,NewToken,Num_Line);
    }


    string Update_Tokens(string lexeme)
    {
        string token = default;
        string[] newTL_Token = TL_Token.ToArray();
        switch (lexeme)
        {
            case "int":
                TL_Token.RemoveAt(newTL_Token.Length - 1);
                TL_Token.Add("Tipo");
                token = "Tipo";
                break;
            case "float":
                TL_Token.RemoveAt(newTL_Token.Length - 1);
                TL_Token.Add("Tipo");
                token = "Tipo";
                break;
            default:
                TL_Token.RemoveAt(newTL_Token.Length - 1);
                TL_Token.Add("Identificador");
                token = "Identificador";
                break;
        }
        return token;
    }


    [ContextMenu("Clean")]
    void Clean()
    {
        TL_Lexeme = new List<string>();
        TL_Token = new List<string>();
        GameObject[] newleme = ObjLexeme.ToArray();
        GameObject[] newtoken = ObjToken.ToArray();
        GameObject[] newtype = ObjType.ToArray();
        GameObject[] newline = ObjNo_Line.ToArray();

        #region Reset_Symbols_Table

        for (int i = Lexeme.ToArray().Length-1; i > 8; i--)
        {
            Lexeme.RemoveAt(i);
        }
        
        for (int i = Token.ToArray().Length - 1; i > 8; i--)
        {
            Token.RemoveAt(i);
        }
        
        for (int i = Type.ToArray().Length - 1; i > 8; i--)
        {
            Type.RemoveAt(i);
        }
        
        No_Line = new List<string>();
        #endregion

        for (int i = 0; i < newleme.Length; i++)
        {
            Destroy(newleme[i]);
            Destroy(newtoken[i]);
            Destroy(newtype[i]);
            Destroy(newline[i]);
        }

        ObjLexeme = new List<GameObject>();
        ObjToken = new List<GameObject>();
        ObjType = new List<GameObject>();
        ObjNo_Line = new List<GameObject>();

    }
}