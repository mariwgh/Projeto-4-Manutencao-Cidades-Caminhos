﻿//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj4
{
    public class Ligacao : IComparable<Ligacao>
    {
        string origem, destino;
        int distancia;

        public int CompareTo(Ligacao other)
        {
            return (origem + destino).CompareTo(other.origem + other.destino);
        }
    }
}
