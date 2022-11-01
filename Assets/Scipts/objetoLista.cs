using System.Collections.Generic;
using UnityEngine;
using NativeSerializableDictionary;

[CreateAssetMenu(fileName = "Fila 0", menuName = "DatosFila")]
public class objetoLista : ScriptableObject
{
    [SerializeField]
    public SerializableDictionary<string, DatosCasilla> Rutas;
}
[System.Serializable]
public class DatosCasilla
{
    public int LineaDes;
    public string TokenResultado;
    public List<string> listaTokenRetroceso;
}

