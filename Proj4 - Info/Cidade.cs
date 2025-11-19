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
            this.excluido = false;
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
        public bool Excluido { get; set; }
        public ListaSimples<Ligacao> Ligacoes { get => ligacoes; }
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }


        public override string ToString()
        {
            return Nome.TrimEnd() + " (" + ligacoes.QuantosNos + ")";
        }
        public int CompareTo(Cidade outraCid)
        {
            return Nome.CompareTo(outraCid.Nome);
        }


        public void LerRegistro(BinaryReader arquivo, long qualRegistro) { }
        public void GravarRegistro(BinaryWriter arquivo) { }

    }
}
