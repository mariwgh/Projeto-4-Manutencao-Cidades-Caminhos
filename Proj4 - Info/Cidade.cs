//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using AgendaAlfabetica;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proj4
{
    public class Cidade : IComparable<Cidade>, IRegistro
    {
        string nome;
        double x, y;
        ListaSimples<Ligacao> ligacoes = new ListaSimples<Ligacao>();

        const int tamanhoNome = 25;
        const int tamanhoRegistro = tamanhoNome + (2 * sizeof(double));

        public Cidade()
        {
            this.Nome = "";
            this.x = 0;
            this.y = 0;
        }

        public Cidade(string nome, double x, double y)
        {
            this.Nome = nome;
            this.x = x;
            this.y = y;
        }

        public Cidade(string nome)
        {
            this.Nome = nome;
        }


        public string Nome
        {
            get => nome;
            set => nome = value.PadRight(tamanhoNome, ' ').Substring(0, tamanhoNome);
        }
        public int TamanhoRegistro { get => tamanhoRegistro; }
        public ListaSimples<Ligacao> Ligacoes { get => ligacoes; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public bool Excluido { get; internal set; }

        public override string ToString()
        {
            return Nome.TrimEnd() + " (" + ligacoes.QuantosNos + ")";
        }
        public int CompareTo(Cidade outraCid)
        {
            return Nome.CompareTo(outraCid.Nome);
        }


        public void LerRegistro(BinaryReader arquivo, long qualRegistro) 
        {
            long posicaoEmBytes = qualRegistro * TamanhoRegistro;

            arquivo.BaseStream.Seek(posicaoEmBytes, SeekOrigin.Begin);

            nome = new string(arquivo.ReadChars(tamanhoNome));
            x = arquivo.ReadDouble();
            y = arquivo.ReadDouble();
        }

        public void GravarRegistro(BinaryWriter arquivo) 
        {
            arquivo.Write(Encoding.Default.GetBytes(nome));
            arquivo.Write(x);
            arquivo.Write(y);
        }

    }
}
