using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analisis2 : MonoBehaviour
{
    [SerializeField] List<objetoLista> Tabla;
    [SerializeField] ControlTablas CT;
    [SerializeField] Analisis3 An3;
    public List<string> LisTokens;
    List<int> PosLinea;
    List<string> Lexemas;
    public List<string> EntradaTokens;
    public List<int> LisLinea;
    bool EncontroError,TipoEncontrado;
    string tipo, identi;
    public void IniAnalisis(List<string> LTokens,List<int> Llineas)
    {
        LisTokens = new List<string>();
        PosLinea = new List<int>();

        PosLinea.Add(0);
        EntradaTokens = LTokens;
        LisLinea = Llineas;
        Lexemas = CT.RegresarListaLexemas();
        EncontroError = false;
        if (EntradaTokens.Count>0)
        {
            
            LisLinea.Add(LisLinea[LisLinea.Count-1]);
            EntradaTokens.Add("FIN");
            for (int i = 0; i < 1000; i++)
            {

                objetoLista p = Tabla[PosLinea[PosLinea.Count - 1]];
                if (EntradaTokens[0] == "FIN" && PosLinea[1] == 1)
                {
                    break;
                }
                else if (p.Rutas.ContainsKey(EntradaTokens[0]))
                {
                    Debug.Log(EntradaTokens[0] + " " + PosLinea[PosLinea.Count - 1]);
                    Desplasamineto(p);
                }
                else if (p.Rutas.ContainsKey("BACIO"))
                {
                    LisTokens.Add("BACIO");
                    PosLinea.Add(p.Rutas.GetValueOrDefault("BACIO").Value.LineaDes);
                }
                else if (p.Rutas.ContainsKey("TODO"))
                {
                    Debug.Log(""+ p.Rutas.GetValueOrDefault("TODO").Value.TokenResultado);
                    Retroceso(p);
                }
                else
                {
                    string Tokens = "", men;
                    var liskeys = Tabla[PosLinea[PosLinea.Count - 1]].Rutas.Keys;
                    foreach (string item in liskeys)
                    {
                        Tokens += item + ",";
                    }
                    men = Tokens.Trim(',');
                    CT.AgregarMensaje("ERROR","Se esperaba unos de estos tokens " + men, "" + LisLinea[0]);
                    EncontroError = true;
                }
                if (EncontroError == true)
                {
                    break;
                }
            }
        }

        if (EncontroError == true)
        {
            CT.AgregarMensaje("ERROR", "No se pudo terminar el analisis ", "");
        }
        else
        {
            CT.AgregarMensaje("Mensage", "Se termino el analisis 2", "");
            An3.Despasamientos();
        }
    }
    void Retroceso(objetoLista Pos)
    {
        List<string> tokens=Pos.Rutas.GetValueOrDefault("TODO").Value.listaTokenRetroceso;
        int NumR = tokens.Count;
        string TokenRetro = Pos.Rutas.GetValueOrDefault("TODO").Value.TokenResultado;
        bool Paso = true;
        for (int i=1;i<=NumR;i++)
        {
            if (tokens[tokens.Count-i] == LisTokens[LisTokens.Count - 1])
            {
                PosLinea.RemoveAt(PosLinea.Count - 1);
                LisTokens.RemoveAt(LisTokens.Count - 1);
            }
            else
            {
                CT.AgregarMensaje("ERROR"," Lista de tokens incompleta para "+TokenRetro, "");
                Paso = false;
                EncontroError = true;
                break;
            }
        }
        if (Paso==true)
        {
            EntradaTokens.Insert(0,TokenRetro);
            Lexemas.Insert(0,"");
            LisLinea.Insert(0, LisLinea[0]);
        }
    }
    void Desplasamineto(objetoLista Pos)
    {
        DeclaracionBariable(EntradaTokens[0]);
        LisTokens.Add(EntradaTokens[0]);
        PosLinea.Add(Pos.Rutas.GetValueOrDefault(EntradaTokens[0]).Value.LineaDes);
        EntradaTokens.RemoveAt(0);
        LisLinea.RemoveAt(0);
        Lexemas.RemoveAt(0);
    }
    void DeclaracionBariable(string Token)
    {
        if (Token=="Tipo")
        {
            if (TipoEncontrado==false)
            {
                tipo = Lexemas[0];
                TipoEncontrado = true;
            }
        }
        else if (Token=="IDENTIFICADOR" && TipoEncontrado==true)
        {
            identi = Lexemas[0];
            if (CT.VariableDeclarada(identi))
            {
                CT.AgregarMensaje("ERROR","Varible ya declarado con anterioridad: "+identi,""+LisLinea[0]);
            }
            else
            {
                CT.AgregarTipo(identi,tipo);
                if (EntradaTokens[1] == "PA")
                {
                    CT.CambiarToken(Lexemas[0], "Metodo");
                }
            }
            tipo = "";
            identi = "";
            TipoEncontrado = false;
        }
        else if (Token == "IDENTIFICADOR")
        {
            if (!CT.VariableDeclarada(Lexemas[0]))
            {
                if (EntradaTokens[1]== "PA" )
                {
                    CT.CambiarToken(Lexemas[0], "Metodo");
                }
                else if (LisTokens[LisTokens.Count - 1] == "USING")
                {
                    CT.CambiarToken(Lexemas[0],"LIBRERIA");
                }
                else if (LisTokens[LisTokens.Count - 1] == "CLASS")
                {
                    CT.CambiarToken(Lexemas[0], "CLASE");
                    CT.AgregarTipo(Lexemas[0], "CLASE");
                }
                else
                {
                    CT.AgregarMensaje("ERROR", "Varible no declarada: " + Lexemas[0], "" + LisLinea[0]);
                }
            }
        }
    }
}