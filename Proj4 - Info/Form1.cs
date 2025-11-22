//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using AgendaAlfabetica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Proj4
{
    public partial class Form1 : Form
    {
        //cidades.dat: nome cordenadax cordenaday
        //grafoonibus.txt: nomeOrigem nomeDestino distancia

        Arvore<Cidade> arvoreBuscaBinariaBalanceadaAVL = new Arvore<Cidade>();

        const string arqCidades = "cidades.dat";
        const string arqGrafosLigacoes = "GrafoOnibusSaoPaulo.txt";
        private Image mapa = Properties.Resources.SaoPaulo_MesoMicroSemMunicip;


        public Form1()
        {
            InitializeComponent();
        }


        // CARREGAMENTO 

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LerArquivoDeCidades();
                pnlArvore.Refresh();

                LerArquivoDeCaminhos();
                pnlArvore.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }


        private void LerArquivoDeCidades()
        {
            if (!File.Exists(arqCidades))
            {
                MessageBox.Show("Arquivo de Cidades não encontrado!");
                return;
            }

            //em cada no da arvore, cada cidade tem que ter um atributo de listasimples

            arvoreBuscaBinariaBalanceadaAVL.LerArquivoDeRegistros(arqCidades);
        }

        private void LerArquivoDeCaminhos()
        {
            if (!File.Exists(arqGrafosLigacoes))
            {
                MessageBox.Show("Arquivo de Caminhos não encontrado!");
                return;
            }

            string[] linhas = File.ReadAllLines(arqGrafosLigacoes);

            foreach (string linha in linhas)
            {
                string[] partes = linha.Split(';'); //separa em 3: nomeOrigem nomeDestino distancia

                string nomeOrigem = partes[0].Trim();
                string nomeDestino = partes[1].Trim();
                int distancia = int.Parse(partes[2].Trim());

                //inclusão bidirecional (IDA e VOLTA)
                Ligacao ida = new Ligacao(nomeOrigem, nomeDestino, distancia);
                Ligacao volta = new Ligacao(nomeDestino, nomeOrigem, distancia);

                //caminhos de cada cidade é info da cidade
                Cidade cidadeOrigemParaBusca = new Cidade(nomeOrigem);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeOrigemParaBusca))
                {
                    arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.InserirAposFim(ida);
                }

                Cidade cidadeDestinoParaBusca = new Cidade(nomeDestino);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeDestinoParaBusca))
                {
                    arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.InserirAposFim(volta);
                }
            }
        }


        // EVENTOS CLICK

        //verificar (evento Leave do textBox) se a cidade não existe
        private void txtNomeCidade_Leave(object sender, EventArgs e)
        {
            string nomeBuscado = txtNomeCidade.Text.Trim();

            //se algo foi digitado antes de sair de foco
            if (!(string.IsNullOrWhiteSpace(nomeBuscado)))
            {
                Cidade cidadeChave = new Cidade(nomeBuscado);

                //true se a cidade existe
                bool cidadeEncontrada = arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeChave);

                if (!cidadeEncontrada) //se a cidade NÃO existe
                {
                    MessageBox.Show($"A cidade '{nomeBuscado}' não está cadastrada.");

                    //habilitar botão Incluir
                    btnIncluirCidade.Enabled = true;

                    //limpa os campos X e Y para uma nova inclusão
                    udX.Value = 0;
                    udY.Value = 0;
                }
                else //se a cidade existe
                {
                    btnIncluirCidade.Enabled = false;

                    Cidade dadosDaCidade = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;
                    udX.Value = (decimal)dadosDaCidade.X;
                    udY.Value = (decimal)dadosDaCidade.Y;
                }
            }
        }
        private void btnIncluirCidade_Click(object sender, EventArgs e)
        {
            string nome = txtNomeCidade.Text.Trim();
            double x = (double)udX.Value;
            double y = (double)udY.Value;

            if (btnIncluirCidade.Enabled == true)
            {
                Cidade novaCidade = new Cidade(nome, x, y);

                arvoreBuscaBinariaBalanceadaAVL.IncluirNovoDado(novaCidade);
                btnIncluirCidade.Enabled = false;

                pnlArvore.Refresh();
                MessageBox.Show($"Cidade '{nome}' incluída com sucesso!");
            }
        }

        private void btnBuscarCidade_Click(object sender, EventArgs e)
        {
            string nome = txtNomeCidade.Text.Trim();

            if (!(string.IsNullOrEmpty(nome)))
            {
                Cidade busca = new Cidade(nome);

                if (arvoreBuscaBinariaBalanceadaAVL.Existe(busca))
                {
                    udX.Value = (decimal)arvoreBuscaBinariaBalanceadaAVL.Atual.Info.X;
                    udY.Value = (decimal)arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Y;

                    dgvLigacoes.Rows.Clear();

                    if (arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes != null)
                    {
                        foreach (Ligacao ligacao in arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.Listar())
                        {
                            dgvLigacoes.Rows.Add(       //add linha
                                ligacao.Destino.Trim(), //coluna 1: nome da cidade destino
                                ligacao.Distancia       //coluna 2: distancia
                            );
                        }
                    }

                    pnlArvore.Refresh();
                }
                else
                {
                    MessageBox.Show($"Cidade '{nome}' não encontrada.");
                    udX.Value = 0;
                    udY.Value = 0;
                    dgvLigacoes.Rows.Clear();
                }
            }
        }

        private void btnAlterarCidade_Click(object sender, EventArgs e)
        {
            if (arvoreBuscaBinariaBalanceadaAVL.Atual.Info != null)
            {
                arvoreBuscaBinariaBalanceadaAVL.Atual.Info.X = (double)udX.Value;
                arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Y = (double)udY.Value;
            }

            pnlArvore.Refresh();
            MessageBox.Show($"Coordenadas da cidade '{arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Nome}' alteradas com sucesso!");
        }

        private void btnExcluirCidade_Click(object sender, EventArgs e)
        {
            if (arvoreBuscaBinariaBalanceadaAVL.Atual.Info != null)
            {
                if (arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.QuantosNos > 0)
                {
                    MessageBox.Show("Não é possível excluir uma cidade que possui caminhos cadastrados.");
                    return;
                }

                arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Excluido = true;
                arvoreBuscaBinariaBalanceadaAVL.Atual.Info = null;

                pnlArvore.Refresh();
                MessageBox.Show("Cidade excluída logicamente com sucesso!");
            }
        }


        private void btnIncluirCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null || cidadeAtual.Excluido || txtNovoDestino.SelectedItem == null) return;

            string nomeOrigem = cidadeAtual.Nome;
            string nomeDestino = txtNovoDestino.SelectedItem.ToString();
            int distancia = (int)udDistancia.Value;

            if (distancia <= 0) return;

            bool idaAdicionada = TentarAdicionarLigacaoUnidirecional(nomeOrigem, nomeDestino, distancia);
            bool voltaAdicionada = TentarAdicionarLigacaoUnidirecional(nomeDestino, nomeOrigem, distancia);

            if (idaAdicionada && voltaAdicionada)
            {
                MessageBox.Show("Ligação bidirecional adicionada com sucesso!");
            }
            else if (!idaAdicionada && !voltaAdicionada)
            {
                MessageBox.Show("A ligação já existe.");
            }

            AtualizarControlesUI();
            pnlArvore.Refresh();
        }

        private void btnExcluirCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null || dgvLigacoes.SelectedRows.Count == 0) return;

            Ligacao ligacaoSelecionada = dgvLigacoes.SelectedRows[0].DataBoundItem as Ligacao;
            if (ligacaoSelecionada == null) return;

            string nomeOrigem = cidadeAtual.Nome;
            string nomeDestino = ligacaoSelecionada.Destino.Trim();

            if (MessageBox.Show($"Tem certeza que deseja excluir a ligação para '{nomeDestino}'?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool idaRemovida = TentarRemoverLigacaoUnidirecional(nomeOrigem, nomeDestino);
                bool voltaRemovida = TentarRemoverLigacaoUnidirecional(nomeDestino, nomeOrigem);

                if (idaRemovida && voltaRemovida)
                {
                    MessageBox.Show("Ligação bidirecional removida com sucesso!");
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro ao remover uma das direções da ligação.");
                }
            }

            AtualizarControlesUI();
            pnlArvore.Refresh();
        }

        // busca de rotas (dijkstra)

        private Dictionary<string, (int distancia, string anterior)> Dijkstra(string nomeInicio)
        {
            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            var nosGrafo = todasCidades.Where(c => !c.Excluido).ToDictionary(c => c.Nome, c => c);

            Dictionary<string, int> distancias = nosGrafo.ToDictionary(k => k.Key, v => int.MaxValue);
            Dictionary<string, string> anteriores = nosGrafo.ToDictionary(k => k.Key, v => (string)null);

            List<string> nosNaoVisitados = nosGrafo.Keys.ToList();

            if (distancias.ContainsKey(nomeInicio))
            {
                distancias[nomeInicio] = 0;
            }
            else
            {
                return null;
            }

            while (nosNaoVisitados.Count > 0)
            {
                string noAtualNome = null;
                int menorDistancia = int.MaxValue;

                foreach (string no in nosNaoVisitados)
                {
                    if (distancias.ContainsKey(no) && distancias[no] < menorDistancia)
                    {
                        menorDistancia = distancias[no];
                        noAtualNome = no;
                    }
                }

                if (noAtualNome == null) break;

                nosNaoVisitados.Remove(noAtualNome);

                Cidade cidadeAtualGrafo = nosGrafo[noAtualNome];

                // Relaxamento das arestas
                cidadeAtualGrafo.Ligacoes.IniciarPercursoSequencial();
                while (cidadeAtualGrafo.Ligacoes.PodePercorrer())
                {
                    Ligacao ligacao = cidadeAtualGrafo.Ligacoes.Atual.Info;
                    string vizinhoNome = ligacao.Destino.Trim();
                    int peso = ligacao.Distancia;

                    if (nosGrafo.ContainsKey(vizinhoNome) && distancias[vizinhoNome] != int.MaxValue)
                    {
                        int novaDistancia = distancias[noAtualNome] + peso;

                        if (novaDistancia < distancias[vizinhoNome])
                        {
                            distancias[vizinhoNome] = novaDistancia;
                            anteriores[vizinhoNome] = noAtualNome;
                        }
                    }
                }
            }

            return nosGrafo.Keys.ToDictionary(
                key => key,
                key => (distancias[key], anteriores[key])
            );
        }

        private void btnBuscarCaminho_Click(object sender, EventArgs e)
        {
            rotaEncontrada.Clear();
            dgvRotas.Rows.Clear();
            lbDistanciaTotal.Text = "Distância Total: N/A";

            if (cidadeAtual == null || cidadeAtual.Excluido || cbxCidadeDestino.SelectedItem == null) return;

            string nomeOrigem = cidadeAtual.Nome;
            string nomeDestino = cbxCidadeDestino.SelectedItem.ToString();

            if (nomeOrigem == nomeDestino) return;

            var resultado = Dijkstra(nomeOrigem);

            if (resultado == null || !resultado.ContainsKey(nomeDestino)) return;

            int distanciaTotal = resultado[nomeDestino].distancia;

            if (distanciaTotal == int.MaxValue)
            {
                MessageBox.Show($"Não existe caminho de ônibus de '{nomeOrigem}' para '{nomeDestino}'.", "Caminho Não Encontrado");
                pnlArvore.Refresh();
                return;
            }

            // Reconstroi o caminho (do destino para a origem)
            Stack<string> caminhoInverso = new Stack<string>();
            string atualNome = nomeDestino;
            while (atualNome != null)
            {
                caminhoInverso.Push(atualNome);
                atualNome = resultado[atualNome].anterior;
            }

            // Popula o DataGridView e a lista de rota (da origem para o destino)
            string ultimoNome = null;
            while (caminhoInverso.Count > 0)
            {
                string nomePasso = caminhoInverso.Pop();
                rotaEncontrada.Add(nomePasso);

                int distDoAnterior = 0;
                if (ultimoNome != null)
                {
                    // Busca a distância do trecho anterior (ultimoNome -> nomePasso)
                    Cidade origemTrechoParaBusca = new Cidade(ultimoNome);
                    if (arvoreBuscaBinariaBalanceadaAVL.Buscar(origemTrechoParaBusca))
                    {
                        Ligacao buscaLig = new Ligacao(ultimoNome, nomePasso, 0);
                        if (arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.Buscar(buscaLig))
                        {
                            distDoAnterior = arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.Atual.Info.Distancia;
                        }
                    }
                }

                dgvRotas.Rows.Add(nomePasso, distDoAnterior);
                ultimoNome = nomePasso;
            }

            lbDistanciaTotal.Text = $"Distância Total: {distanciaTotal} km";
            pnlArvore.Refresh();
        }


        // DESENHO DO MAPA

        private PointF MapearCoordenada(double x, double y)
        {
            // Mapeia coordenadas (0.0 a 1.0) para as dimensões atuais do Panel
            float posX = (float)x * pnlArvore.Width;
            float posY = (float)y * pnlArvore.Height;
            return new PointF(posX, posY);
        }

        private void pnlArvore_Paint2(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mapa, 0, 0, pnlArvore.Width, pnlArvore.Height);

            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            Dictionary<string, PointF> posicoesCidades = new Dictionary<string, PointF>();

            // 2. Desenha Ligações (Arestas) em cinza claro
            Pen penLigacao = new Pen(Color.LightGray, 1);

            foreach (Cidade cidade in todasCidades.Where(c => !c.Excluido))
            {
                PointF posOrigem = MapearCoordenada(cidade.X, cidade.Y);
                posicoesCidades[cidade.Nome] = posOrigem;

                // Desenha a ligação de A -> B, apenas se A < B (evita desenho duplicado)
                cidade.Ligacoes.IniciarPercursoSequencial();
                while (cidade.Ligacoes.PodePercorrer())
                {
                    Ligacao ligacao = cidade.Ligacoes.Atual.Info;

                    string nomeA = cidade.Nome;
                    string nomeB = ligacao.Destino.Trim();

                    if (string.Compare(nomeA, nomeB) < 0)
                    {
                        Cidade buscaDestino = new Cidade(ligacao.Destino);
                        if (arvoreBuscaBinariaBalanceadaAVL.Buscar(buscaDestino) && !arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Excluido)
                        {
                            PointF posDestino = MapearCoordenada(arvoreBuscaBinariaBalanceadaAVL.Atual.Info.X, arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Y);
                            e.Graphics.DrawLine(penLigacao, posOrigem, posDestino);
                        }
                    }
                }
            }

            // 3. Desenha a Rota do Dijkstra (em destaque)
            if (rotaEncontrada.Count > 1)
            {
                Pen penRota = new Pen(Color.Red, 3);
                for (int i = 0; i < rotaEncontrada.Count - 1; i++)
                {
                    string nomeA = rotaEncontrada[i];
                    string nomeB = rotaEncontrada[i + 1];

                    if (posicoesCidades.ContainsKey(nomeA) && posicoesCidades.ContainsKey(nomeB))
                    {
                        PointF posA = posicoesCidades[nomeA];
                        PointF posB = posicoesCidades[nomeB];
                        e.Graphics.DrawLine(penRota, posA, posB);
                    }
                }
            }

            // 4. Desenha as Cidades (Vértices)
            float raio = 5f;
            Font fonte = new Font("Arial", 8);

            foreach (Cidade cidade in todasCidades.Where(c => !c.Excluido))
            {
                PointF pos = posicoesCidades[cidade.Nome];
                Brush brushUsado = Brushes.Blue;

                // Prioridade de cor: Rota > Cidade Atual > Normal
                if (rotaEncontrada.Contains(cidade.Nome))
                {
                    brushUsado = Brushes.Green;
                }
                if (cidadeAtual != null && cidade.Nome == cidadeAtual.Nome)
                {
                    brushUsado = Brushes.Red;
                }

                // Desenha o círculo
                e.Graphics.FillEllipse(brushUsado, pos.X - raio, pos.Y - raio, 2 * raio, 2 * raio);
                e.Graphics.DrawString(cidade.Nome, fonte, brushUsado, pos.X + raio, pos.Y - raio);
            }
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            arvoreBuscaBinariaBalanceadaAVL.Desenhar(pnlArvore);
        }


        // FECHAR

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 1. Grava a Árvore de Cidades no arquivo binário (em ordem)
                arvoreBuscaBinariaBalanceadaAVL.GravarArquivoDeRegistros(arqCidades);

                // 2. Grava as Ligações de volta no arquivo de texto
                GravarArquivoDeLigacoes(arqGrafosLigacoes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os dados: {ex.Message}\nO programa será encerrado sem salvar as últimas alterações.", "Erro de Salvamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void GravarArquivoDeLigacoes(string nomeArquivo)
        {
            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            List<string> linhasParaGravar = new List<string>();
            HashSet<(string, string)> ligacoesJaGravadas = new HashSet<(string, string)>();

            foreach (Cidade cidade in todasCidades.Where(c => !c.Excluido))
            {
                cidade.Ligacoes.IniciarPercursoSequencial();
                while (cidade.Ligacoes.PodePercorrer())
                {
                    Ligacao ligacao = cidade.Ligacoes.Atual.Info;

                    string nomeA = cidade.Nome;
                    string nomeB = ligacao.Destino.Trim();

                    // Normaliza a ordem para evitar duplicação (A;B é o mesmo que B;A)
                    string menor = string.Compare(nomeA, nomeB) < 0 ? nomeA : nomeB;
                    string maior = string.Compare(nomeA, nomeB) < 0 ? nomeB : nomeA;

                    if (!ligacoesJaGravadas.Contains((menor, maior)))
                    {
                        linhasParaGravar.Add($"{nomeA};{nomeB};{ligacao.Distancia}");
                        ligacoesJaGravadas.Add((menor, maior));
                    }
                }
            }

            File.WriteAllLines(nomeArquivo, linhasParaGravar);
        }


        // NAOSEIOQEISSO

        Cidade cidadeAtual = null;
        List<string> rotaEncontrada = new List<string>();

        private void AtualizarControlesUI()
        {
            if (cidadeAtual != null)
            {
                txtNomeCidade.Text = cidadeAtual.Nome;
                udX.Value = (decimal)cidadeAtual.X;
                udY.Value = (decimal)cidadeAtual.Y;

                dgvLigacoes.DataSource = cidadeAtual.Ligacoes.Listar();
                dgvLigacoes.Refresh();

                AtualizarComboBoxDestinoLigacao(cidadeAtual.Nome);

                btnAlterarCidade.Enabled = !cidadeAtual.Excluido;
                btnExcluirCidade.Enabled = !cidadeAtual.Excluido && cidadeAtual.Ligacoes.EstaVazia;
                btnIncluirCaminho.Enabled = !cidadeAtual.Excluido;

                //txtStatus.Text = cidadeAtual.Excluido ? "CIDADE EXCLUÍDA" : $"Caminhos: {cidadeAtual.Ligacoes.QuantosNos}";
            }
            else
            {
                txtNomeCidade.Text = "";
                udX.Value = 0;
                udY.Value = 0;
                dgvLigacoes.DataSource = null;
                //txtStatus.Text = "Nenhuma cidade selecionada";
                btnAlterarCidade.Enabled = false;
                btnExcluirCidade.Enabled = false;
                btnIncluirCaminho.Enabled = false;
            }
        }
        private void AtualizarComboBoxDestino()
        {
            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            var nomesValidos = todasCidades
                .Where(c => !c.Excluido)
                .Select(c => c.Nome)
                .ToList();

            cbxCidadeDestino.DataSource = nomesValidos;
        }
        private void AtualizarComboBoxDestinoLigacao(string nomeOrigem)
        {
            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            var destinosPossiveis = todasCidades
                .Where(c => !c.Excluido && c.Nome != nomeOrigem)
                .Select(c => c.Nome)
                .ToList();

            txtNovoDestino.Text = destinosPossiveis.ToString();
        }

    }
}
