//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaAlfabetica;

namespace Proj4
{
    public class Ligacao : IComparable<Ligacao>
    {
        string origem, destino;
        int distancia;

        public Ligacao() { }

        public Ligacao(string origem, string destino, int distancia)
        {
            this.origem = origem;
            this.destino = destino;
            this.distancia = distancia;
        }

        public string Origem { get => origem; set => origem = value; }
        public string Destino { get => destino; set => destino = value; }
        public int Distancia { get => distancia; set => distancia = value; }

        public int CompareTo(Ligacao other)
        {
            return (origem + destino).ToUpper().CompareTo((other.origem + other.destino).ToUpper());
        }

        public override string ToString()
        {
            return $"{Destino} ({Distancia} km)";
        }
    }
}