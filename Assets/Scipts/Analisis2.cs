using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analisis2 : MonoBehaviour
{
    [SerializeField] List<objetoLista> Tabla;
    [SerializeField] ControlTablas CT;
    List<string> LisTokens;
    List<int> PosLinea;
    string Bariable;
    List<string> EntradaTokens;
    public List<int> LisLinea;
    bool EncontroError;
    public void IniAnalisis(List<string> LTokens,List<int> Llineas)
    {
        LisTokens = new List<string>();
        PosLinea = new List<int>();

        PosLinea.Add(0);
        EntradaTokens = LTokens;
        LisLinea = Llineas;
        EncontroError = false;
        if (EntradaTokens.Count>0)
        {
            LisLinea.Add(LisLinea[LisLinea.Count-1]);
            EntradaTokens.Add("FIN");
            for (int i = 0; i < 150; i++)
            {
                objetoLista p = Tabla[PosLinea[PosLinea.Count - 1]];
                if (EntradaTokens[0] == "FIN" && PosLinea[1] == 1)
                {
                    break;
                }
                else if (p.Rutas.ContainsKey(EntradaTokens[0]))
                {
                    //Debug.Log(EntradaTokens[0] + " " + PosLinea[PosLinea.Count - 1]);
                    Desplasamineto(p);
                }
                else if (p.Rutas.ContainsKey("Bacio"))
                {
                    LisTokens.Add("Bacio");
                    PosLinea.Add(p.Rutas.GetValueOrDefault("Bacio").Value.LineaDes);
                }
                else if (p.Rutas.ContainsKey("TODO"))
                {
                    //Debug.Log(1);
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
                    CT.AgregarMensaje("ERROR", "Se esperaba unos de estos tokens " + men, "" + LisLinea[0]);
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
            else if (LisTokens[LisTokens.Count - 1]=="Bacio")
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
            LisLinea.Insert(0, LisLinea[0]);
        }
    }
    void Desplasamineto(objetoLista Pos)
    {
        LisTokens.Add(EntradaTokens[0]);
        PosLinea.Add(Pos.Rutas.GetValueOrDefault(EntradaTokens[0]).Value.LineaDes);
        EntradaTokens.RemoveAt(0);
        LisLinea.RemoveAt(0);
    }
    void PonerTipo()
    {

    }
    void DeclaracionBariable()
    {

    }
}