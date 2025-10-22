using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaAlfabetica
{
    public class ListaSimples<Dado>
        where Dado : IComparable<Dado>
    {
        private NoLista<Dado> primeiro;
        private NoLista<Dado> ultimo;
        private NoLista<Dado> anterior, atual;
        private int quantosNos;

        private bool primeiroAcessoDoPercurso;

        public ListaSimples()
        {
            primeiro = ultimo = anterior = atual = null;
            quantosNos = 0;
            primeiroAcessoDoPercurso = false;
        }

        public void IniciarPercursoSequencial()
        {
            primeiroAcessoDoPercurso = true;
            atual = primeiro;
            anterior = null;
        }

        public bool PodePercorrer()
        {
            if (!primeiroAcessoDoPercurso)
            {
                anterior = atual;
                atual = atual.Prox;
            }
            else
                primeiroAcessoDoPercurso = false;

            return atual != null;
        }
        public void PercorrerLista()
        {
            atual = primeiro;
            while (atual != null)
            {
                Console.WriteLine(atual.Info);
                atual = atual.Prox;
            }
        }

        public bool EstaVazia
        {
            get { return primeiro == null; }
        }

        public NoLista<Dado> Primeiro
        {
            get
            {
                return primeiro;
            }
        }

        public NoLista<Dado> Ultimo
        {
            get
            {
                return ultimo;
            }
        }

        public int QuantosNos
        {
            get { return quantosNos; }
        }

        public NoLista<Dado> Atual
        {
            get
            {
                return atual;
            }
        }

      public Dado InfoInicio()
        {
            if (EstaVazia)
                throw new Exception("Lista vazia! Não é possível acessar o início.");
            return primeiro.Info;
        }
    
    public Dado RemoverOPrimeiro()
    {
      if (EstaVazia)
        throw new Exception("Não é possível remover o primeiro.");
      Dado dado = primeiro.Info;
      primeiro = primeiro.Prox;
      if (primeiro == null)
        ultimo = null; // lista ficou vazia
      quantosNos--;
      return dado;
    }
        public void InserirAntesDoInicio(Dado novoDado)
        {
            var novoNo = new NoLista<Dado>(novoDado, primeiro);

            if (ultimo == null)
                ultimo = novoNo;

            primeiro = novoNo;
        }

        public void InserirAposFim(Dado novoDado)
        {
            var novoNo = new NoLista<Dado>(novoDado, null);

            if (EstaVazia)
                primeiro = novoNo;
            else
                ultimo.Prox = novoNo;
            ultimo = novoNo;
            quantosNos++;
        }

        public List<Dado> Listar()
        {
            List<Dado> aLista = new List<Dado>();
            atual = primeiro;
            while (atual != null)
            {
                aLista.Add(atual.Info);
                atual = atual.Prox;
            }
            return aLista;
        }

        public bool ExisteDado(Dado outroProcurado)
        {
            anterior = null;
            atual = primeiro;
            // Em seguida, é verificado se a lista está vazia. Caso esteja, é
            // retornado false ao local de chamada, indicando que a chave não foi
            // encontrada, e atual e anterior ficam valendo null
            if (EstaVazia)
                return false;
            // a lista não está vazia, possui nós
            // dado procurado é menor que o primeiro dado da lista:
            // portanto, dado procurado não existe
            if (outroProcurado.CompareTo(primeiro.Info) < 0)
                return false;
            // dado procurado é maior que o último dado da lista:
            // portanto, dado procurado não existe
            if (outroProcurado.CompareTo(ultimo.Info) > 0)
            {
                anterior = ultimo;
                atual = null;
                return false;
            }
            
// caso não tenha sido definido que a chave está fora dos limites de
// chaves da lista, vamos procurar no seu interior
// o apontador atual indica o primeiro nó da lista e consideraremos que
// ainda não achou a chave procurada nem chegamos ao final da lista
            bool achou = false;
            bool fim = false;
            // repete os comandos abaixo enquanto não achou o RA nem chegou ao
            // final da lista
            while (!achou && !fim)
                // se o apontador atual vale null, indica final da lista
                if (atual == null)
                    fim = true;
                // se não chegou ao final da lista, verifica o valor da chave atual
                else
                // verifica igualdade entre chave procurada e chave do nó atual
                if (outroProcurado.CompareTo(atual.Info) == 0)
                    achou = true;
                else
                // se chave atual é maior que a procurada, significa que
                // a chave procurada não existe na lista ordenada e, assim,
                // termina a pesquisa indicando que não achou. Anterior
                // aponta o anterior ao atual, que foi acessado por
                // último
                if (atual.Info.CompareTo(outroProcurado) > 0)
                    fim = true;
                else
                {
                    // se não achou a chave procurada nem uma chave > que ela,
                    // então a pesquisa continua, de maneira que o apontador
                    // anterior deve apontar o nó atual e o apontador atual
                    // deve seguir para o nó seguinte
                    anterior = atual;
                    atual = atual.Prox;
                }
            // por fim, caso a pesquisa tenha terminado, o apontador atual
            // aponta o nó onde está a chave procurada, caso ela tenha sido
            // encontrada, ou o nó onde ela deveria estar para manter a
            // ordenação da lista. O apontador anterior aponta o nó anterior
            // ao atual
            return achou; // devolve o valor da variável achou, que indica
        } // se a chave procurada foi ou não encontrado

        public bool InserirEmOrdem(Dado dados)
        {
            if (!ExisteDado(dados)) // existeChave configura anterior e atual
            { // aqui temos certeza de que a chave não existe
                if (EstaVazia) // se a lista está vazia, então o
                    InserirAntesDoInicio(dados); // novo nó é o primeiro da lista
                else
                if (anterior == null && atual != null)
                    InserirAntesDoInicio(dados); // liga novo antes do primeiro
                else
                    InserirNoMeio(dados); // insere entre os nós anterior e atual

                return true;  // significa que incluiu
            }
            return false;   // significa que não incluiu

            //throw new Exception("Aluno já cadastrado!");
        }

        public bool RemoverDado(Dado aExcluir)
        {
            if (ExisteDado(aExcluir)) // existeDado configurou 
            {                         // atual e anterior
                quantosNos--;

                if (atual == primeiro)  // se vamos excluir o 1o nó
                {
                    primeiro = primeiro.Prox;
                    if (primeiro == null)  // esvaziou
                        ultimo = null;
                }
                else
                    if (atual == ultimo)    // se vamos excluir o último nó
                    {
                        ultimo = anterior;
                        ultimo.Prox = null;
                    }
                    else
                    {
                        anterior.Prox = atual.Prox;
                        atual.Prox = null;
                    }
                return true;
            }
            return false;
        }
        private void InserirNoMeio(Dado dados)
        {
            var novo = new NoLista<Dado>(dados, null); // guarda dados no
                                                       // novo nó
            // existeChave() encontrou intervalo de inclusão do novo nó
            anterior.Prox = novo; // liga anterior ao novo
            novo.Prox = atual; // e novo no atual
            if (anterior == ultimo) // se incluiu ao final da lista,
                ultimo = novo; // atualiza o apontador ultimo
            quantosNos++; // incrementa número de nós da lista
        }
      
        public void Ordenar()
        {
            ListaSimples<Dado> ordenada = new ListaSimples<Dado>();
            while (!this.EstaVazia)
            {
                // achar o menor de todos
                // remover o menor de todos
                // incluir o menor de todos já removido na lista ordenada
            }
            this.primeiro = ordenada.primeiro;
            this.ultimo = ordenada.ultimo;
            this.quantosNos = ordenada.quantosNos;
        }

        public int Contar()
        {
            int cont = 0;

            for (atual = primeiro;
                 atual != null;
                 cont++, atual = atual.Prox) ;

            return cont;
        }

        private void InserirAposFim(NoLista<Dado> noAntigo)
        {
            if (EstaVazia)
                primeiro = noAntigo;
            else
                ultimo.Prox = noAntigo;
            ultimo = noAntigo;
            noAntigo.Prox = null;
            quantosNos++;
        }

        public ListaSimples<Dado> Juntar(
            ListaSimples<Dado> outra)
        {
            var lista3 = new ListaSimples<Dado>();
            this.atual = this.primeiro;
            outra.atual = outra.primeiro;

            NoLista<Dado> aux = null;
            while (this.atual != null && outra.atual != null)
            {
                if (this.atual.Info.CompareTo(
                    outra.atual.Info) < 0)
                {
                    aux = this.atual.Prox;
                    lista3.InserirAposFim(this.atual);
                    this.atual = aux;
                    this.quantosNos--;
                }
                else
                  if (outra.atual.Info.CompareTo(
                       this.atual.Info) < 0)
                {
                    aux = outra.atual.Prox;
                    lista3.InserirAposFim(outra.atual);
                    outra.atual = aux;
                    outra.quantosNos--;
                }
                else
                {
                    aux = this.atual.Prox;
                    lista3.InserirAposFim(this.atual);
                    this.atual = aux;
                    outra.atual = outra.atual.Prox;
                    this.quantosNos--;
                    outra.quantosNos--;
                }
            }  // while

            if (lista3.ultimo != null) // se houve casamento
                if (this.atual == null) // acabou o percurso da lista1
                {
                    lista3.ultimo.Prox = outra.atual;
                    lista3.ultimo = outra.ultimo;
                    lista3.quantosNos += outra.quantosNos;
                }
                else
                {
                    lista3.ultimo.Prox = this.atual;
                    lista3.ultimo = this.ultimo;
                    lista3.quantosNos += this.quantosNos;
                }
            this.primeiro = this.ultimo = outra.primeiro = outra.ultimo = null;
            this.quantosNos = outra.quantosNos = 0;
            return lista3;
        }

        public void Inverter()
        {
            if (!EstaVazia)
            {
                NoLista<Dado> um = primeiro;
                NoLista<Dado> dois = um.Prox;
                while (dois != null)
                {
                    NoLista<Dado> tres = dois.Prox;
                    dois.Prox = um;
                    um = dois;
                    dois = tres;
                }
                ultimo = primeiro;
                primeiro = um;
                ultimo.Prox = null;
            }
        }
    }
}
