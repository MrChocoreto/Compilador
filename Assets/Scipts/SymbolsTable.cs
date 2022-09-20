using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolsTable : MonoBehaviour{

    #region Variables

    [Header("-----SymbolsTable-----")]
    [SerializeField] List<string> Lexeme;
    [SerializeField] List<string> Token;
    [SerializeField] List<string> Type;
    [SerializeField] List<string> No_Line;

    [Space(20)]
    [Header("-----TokensList-----")]
    [SerializeField] List<string> TL_Lexeme;
    [SerializeField] List<string> TL_Token;
    
    [Space(20)]
    [Header("-----AddElements-----")]
    [SerializeField] Element_SymbolsTable AddElement;
    [SerializeField] GameObject FatherLexeme;
    [SerializeField] GameObject FatherToken;
    [SerializeField] GameObject FatherType;
    [SerializeField] GameObject FatherLine;
    [SerializeField] RectTransform SymbolsTransform;
    [SerializeField] RectTransform TokensTransform;
    [SerializeField] RectTransform ConsoleTransform;

    [Space(20)]
    [Header("-----DeleteElements-----")]
    [SerializeField] public List<GameObject> ObjLexeme;
    [SerializeField] public List<GameObject> ObjToken;
    [SerializeField] public List<GameObject> ObjType;
    [SerializeField] public List<GameObject> ObjNo_Line;

    #endregion


    #region UnityMethods

    private void Start()
    {
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

    #endregion


    #region SymbolsTable_&_TokensList

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

        //Añade los tipos de lexemas, si no es ninguno va a instanciar un vacio
        switch (NewLexeme)
        {
            case "int":
                Type.Add(NewLexeme);
                AddElement.AddNewElement(NewLexeme,FatherType.transform, 30, "Type");
                break;
            case "float":
                Type.Add(NewLexeme);
                AddElement.AddNewElement(NewLexeme, FatherType.transform, 30, "Type");
                break;
            default:
                AddElement.AddNewElement("", FatherType.transform, 30, "Type");
                break;
        }


        No_Line.Add(Num_Line.ToString());
        AddElement.AddNewElement(Num_Line.ToString(), FatherLine.transform, 30, "No_Line");

        SymbolsTransform.sizeDelta = new Vector2(SymbolsTransform.sizeDelta.x, SymbolsTransform.sizeDelta.y+37);

    }


    public void Add_New_Token(string NewLexeme, string NewToken, int Num_Line)
    {
        //Añadido de los elementos a la lista de Tokens
        //y revision de los tokes
        TL_Lexeme.Add(NewLexeme);
        NewToken = Update_Tokens(NewLexeme);

        //Añadido de los elementos a la tabla de simbolos
        Add_new_Lexeme(NewLexeme,NewToken,Num_Line);
    }


    string Update_Tokens(string lexeme)
    {
        string token = default;
        switch (lexeme)
        {
            case "int":
                TL_Token.Add("Tipo");
                token = "Tipo";
                break;
            case "float":
                TL_Token.Add("Tipo");
                token = "Tipo";
                break;
            default:
                TL_Token.Add("Identificador");
                token = "Identificador";
                break;
        }
        return token;
    }

    #endregion


    #region Console

    #endregion


    [ContextMenu("Clean")]
    void Clean()
    {

        #region Reset_Symbols_Table

        GameObject[] newleme = ObjLexeme.ToArray();
        GameObject[] newtoken = ObjToken.ToArray();
        GameObject[] newtype = ObjType.ToArray();
        GameObject[] newline = ObjNo_Line.ToArray();


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

        #endregion


        #region TokenList

        TL_Lexeme = new List<string>();
        TL_Token = new List<string>();

        #endregion

    }

}