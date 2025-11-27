//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using AgendaAlfabetica;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Proj4
{
    public partial class Form1 : Form
    {
        private Arvore<Cidade> arvoreBuscaBinariaBalanceadaAVL = new Arvore<Cidade>();

        private const string arqCidades = "cidades.dat";
        private const string arqGrafosLigacoes = "GrafoOnibusSaoPaulo.txt";

        private Image mapa = Properties.Resources.SaoPaulo_MesoMicroSemMunicip;

        private Cidade cidadeAtual = null;

        private readonly List<string> rotaEncontrada = new List<string>();  // dijkstra da classe Grafo


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LerArquivoDeCidades();
                LerArquivoDeCaminhos();

                AtualizarCombos();
                pnlArvore.Refresh();
                pbMapa.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }


        // metodos de arquivos

        private void LerArquivoDeCidades()
        {
            if (!File.Exists(arqCidades))
            {
                MessageBox.Show("Arquivo de Cidades não encontrado!");
                return;
            }

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
                if (string.IsNullOrWhiteSpace(linha))
                {
                    continue;
                }
                    
                string[] partes = linha.Split(';');

                if (partes.Length < 3)
                {
                    continue;
                }

                string nomeOrigem = partes[0].Trim();
                string nomeDestino = partes[1].Trim();

                if (!int.TryParse(partes[2].Trim(), out int distancia))
                    continue;

                // cria objetos ligacao (ida e volta)
                Ligacao ida = new Ligacao(nomeOrigem, nomeDestino, distancia);
                Ligacao volta = new Ligacao(nomeDestino, nomeOrigem, distancia);

                // origem
                Cidade cidadeOrigemChave = new Cidade(nomeOrigem);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeOrigemChave))
                {
                    arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.InserirAposFim(ida);
                }

                // destino
                Cidade cidadeDestinoChave = new Cidade(nomeDestino);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeDestinoChave))
                {
                    arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Ligacoes.InserirAposFim(volta);
                }
            }
        }

        /// grava cidades em binário (cidades.dat) e ligações em texto (GrafoOnibusSaoPaulo.txt)
        private void GravarDados()
        {
            List<Cidade> lista = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(lista);

            using (FileStream fs = new FileStream(arqCidades, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                foreach (Cidade cidade in lista)
                {
                    if (!cidade.Excluido)
                    {
                        cidade.GravarRegistro(bw);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(arqGrafosLigacoes))
            {
                foreach (Cidade cidade in lista)
                {
                    if (cidade.Excluido) continue;
                    cidade.Ligacoes.IniciarPercursoSequencial();
                    while (cidade.Ligacoes.PodePercorrer())
                    {
                        Ligacao lig = cidade.Ligacoes.Atual.Info;
                        string origem = cidade.Nome.Trim();
                        string destino = lig.Destino.Trim();

                        // só grava a ligação se origem < destino, para não duplicar ida/volta
                        if (origem.CompareTo(destino) < 0)
                        {
                            sw.WriteLine($"{origem};{destino};{lig.Distancia}");
                        }
                    }
                }
            }
        }

        
        // metodos de cidade

        private void txtNomeCidade_Leave(object sender, EventArgs e)
        {
            string nomeBuscado = txtNomeCidade.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBuscado))
                return;

            Cidade chave = new Cidade(nomeBuscado);

            if (!arvoreBuscaBinariaBalanceadaAVL.Existe(chave))
            {
                // se cidade não existe, tem que incluir
                MessageBox.Show($"A cidade '{nomeBuscado}' não está cadastrada.");

                cidadeAtual = null;
                udX.Value = 0;
                udY.Value = 0;
                dgvLigacoes.Rows.Clear();
            }
            else
            {
                // se cidade existe, carrega os dados
                cidadeAtual = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

                udX.Value = (decimal)cidadeAtual.X;
                udY.Value = (decimal)cidadeAtual.Y;

                AtualizarGridLigacoesCidadeAtual();
            }

            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        private void btnIncluirCidade_Click(object sender, EventArgs e)
        {
            string nome = txtNomeCidade.Text.Trim();
            double x = (double)udX.Value;
            double y = (double)udY.Value;

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Digite o nome da cidade.");
                return;
            }

            // x e y já foram setados pelo clique no mapa
            Cidade novaCidade = new Cidade(nome, x, y);
            arvoreBuscaBinariaBalanceadaAVL.IncluirNovoDado(novaCidade);

            cidadeAtual = novaCidade;

            AtualizarCombos();
            AtualizarGridLigacoesCidadeAtual();

            pnlArvore.Refresh();
            pbMapa.Invalidate();

            MessageBox.Show($"Cidade '{nome}' incluída com sucesso!");
        }

        private void btnBuscarCidade_Click(object sender, EventArgs e)
        {
            string nome = txtNomeCidade.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
                return;

            Cidade chave = new Cidade(nome);

            if (arvoreBuscaBinariaBalanceadaAVL.Existe(chave))
            {
                cidadeAtual = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

                udX.Value = (decimal)cidadeAtual.X;
                udY.Value = (decimal)cidadeAtual.Y;

                AtualizarGridLigacoesCidadeAtual();
            }
            else
            {
                MessageBox.Show($"Cidade '{nome}' não encontrada.");

                cidadeAtual = null;
                udX.Value = 0;
                udY.Value = 0;
                dgvLigacoes.Rows.Clear();
            }

            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        private void btnAlterarCidade_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null)
            {
                MessageBox.Show("Nenhuma cidade selecionada para alteração. Busque uma cidade primeiro.");
                return;
            }

            if (cidadeAtual.Excluido)
            {
                MessageBox.Show("Não é possível alterar uma cidade excluída.");
                return;
            }

            try
            {
                double novoX = (double)udX.Value;
                double novoY = (double)udY.Value;

                cidadeAtual.X = novoX;
                cidadeAtual.Y = novoY;

                pbMapa.Invalidate();
                pnlArvore.Refresh();

                MessageBox.Show($"Coordenadas da cidade '{cidadeAtual.Nome.Trim()}' alteradas com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar coordenadas: " + ex.Message);
            }
        }

        private void btnExcluirCidade_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null)
                return;

            if (cidadeAtual.Ligacoes.QuantosNos > 0)
            {
                MessageBox.Show("Não é possível excluir uma cidade que possui caminhos cadastrados.");
                return;
            }

            cidadeAtual.Excluido = true;

            MessageBox.Show("Cidade excluída com sucesso!");

            cidadeAtual = null;
            txtNomeCidade.Clear();
            udX.Value = 0;
            udY.Value = 0;
            dgvLigacoes.Rows.Clear();

            AtualizarCombos();
            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }


        // metodos de caminhos

        private void RemoverLigacaoDaLista(ListaSimples<Ligacao> lista, Ligacao itemParaRemover)
        {
            if (lista.EstaVazia) return;

            // lista temporária p salvar oq NÃO sera excluido
            List<Ligacao> itensParaManter = new List<Ligacao>();

            lista.IniciarPercursoSequencial();
            while (lista.PodePercorrer())
            {
                Ligacao itemAtual = lista.Atual.Info;

                if (itemAtual.CompareTo(itemParaRemover) != 0)
                {
                    itensParaManter.Add(itemAtual);
                }
            }

            // zera a lista original (via Reflection)
            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

            typeof(ListaSimples<Ligacao>).GetField("primeiro", flags)?.SetValue(lista, null);
            typeof(ListaSimples<Ligacao>).GetField("ultimo", flags)?.SetValue(lista, null);
            typeof(ListaSimples<Ligacao>).GetField("quantosNos", flags)?.SetValue(lista, 0);
            typeof(ListaSimples<Ligacao>).GetField("atual", flags)?.SetValue(lista, null);
            typeof(ListaSimples<Ligacao>).GetField("anterior", flags)?.SetValue(lista, null);

            // reinsere
            foreach (var item in itensParaManter)
            {
                lista.InserirAposFim(item);
            }
        }

        private void btnIncluirCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null || cidadeAtual.Excluido)
                return;

            string nomeDestino = txtNovoDestino.Text.Trim();
            int distancia = (int)numericUpDown1.Value;

            if (string.IsNullOrWhiteSpace(nomeDestino))
            {
                MessageBox.Show("Digite o nome da cidade de destino.");
                return;
            }

            Cidade cidadeDestinoChave = new Cidade(nomeDestino);
            if (!arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeDestinoChave) || arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Excluido)
            {
                MessageBox.Show("Cidade de destino não existe ou está excluída.");
                return;
            }

            Cidade cidadeDestino = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

            if (cidadeDestino.Nome == cidadeAtual.Nome)
            {
                MessageBox.Show("Origem e destino não podem ser a mesma cidade.");
                return;
            }

            // inclusão ida e volta
            Ligacao ida = new Ligacao(cidadeAtual.Nome, cidadeDestino.Nome, distancia);
            Ligacao volta = new Ligacao(cidadeDestino.Nome, cidadeAtual.Nome, distancia);

            cidadeAtual.Ligacoes.InserirAposFim(ida);
            cidadeDestino.Ligacoes.InserirAposFim(volta);

            AtualizarGridLigacoesCidadeAtual();
            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        private void btnBuscarCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null)
            {
                MessageBox.Show("Selecione ou digite uma cidade de origem primeiro.");
                return;
            }

            if (cbxCidadeDestino.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma cidade de destino no combo.");
                return;
            }

            string nomeOrigem = cidadeAtual.Nome.Trim();
            string nomeDestino = cbxCidadeDestino.SelectedItem.ToString().Trim();

            if (nomeOrigem == nomeDestino)
            {
                MessageBox.Show("Origem e Destino são a mesma cidade!");
                return;
            }

            Cidade destinoChave = new Cidade(nomeDestino);
            if (!arvoreBuscaBinariaBalanceadaAVL.Existe(destinoChave))
            {
                MessageBox.Show("Cidade de destino não encontrada.");
                return;
            }

            Cidade cidadeDestino = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

            // ALGORITMO DE DIJKSTRA

            var distancias = new Dictionary<string, int>();
            var anteriores = new Dictionary<string, Cidade>();
            var visitados = new HashSet<string>();
            var filaPrioridade = new List<Cidade>();

            // inicializa todas as cidades com distância infinita
            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            foreach (var c in todasCidades)
            {
                distancias[c.Nome.Trim()] = int.MaxValue;
            }

            distancias[nomeOrigem] = 0;
            filaPrioridade.Add(cidadeAtual);

            while (filaPrioridade.Count > 0)
            {
                // ordena para pegar a menor distância 
                filaPrioridade.Sort((a, b) => distancias[a.Nome.Trim()].CompareTo(distancias[b.Nome.Trim()]));

                Cidade u = filaPrioridade[0];
                filaPrioridade.RemoveAt(0);

                if (visitados.Contains(u.Nome.Trim())) continue;
                visitados.Add(u.Nome.Trim());

                if (u.Nome.Trim() == nomeDestino) break;

                // verifica vizinhos
                u.Ligacoes.IniciarPercursoSequencial();
                while (u.Ligacoes.PodePercorrer())
                {
                    Ligacao ligacao = u.Ligacoes.Atual.Info;
                    string nomeVizinho = ligacao.Destino.Trim();

                    if (!visitados.Contains(nomeVizinho))
                    {
                        // busca o objeto do vizinho na árvore
                        Cidade chaveVizinho = new Cidade(nomeVizinho);
                        if (arvoreBuscaBinariaBalanceadaAVL.Existe(chaveVizinho))
                        {
                            Cidade vizinho = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

                            if (distancias[u.Nome.Trim()] != int.MaxValue)
                            {
                                int novaDistancia = distancias[u.Nome.Trim()] + ligacao.Distancia;

                                if (novaDistancia < distancias[nomeVizinho])
                                {
                                    distancias[nomeVizinho] = novaDistancia;
                                    anteriores[nomeVizinho] = u; 

                                    if (!filaPrioridade.Contains(vizinho))
                                    {
                                        filaPrioridade.Add(vizinho);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            dgvRotas.Rows.Clear();
            rotaEncontrada.Clear();

            if (anteriores.ContainsKey(nomeDestino))
            {
                // reconstroi caminho de tras para frente
                List<Cidade> caminho = new List<Cidade>();
                Cidade passo = cidadeDestino;

                while (passo != null)
                {
                    caminho.Add(passo);
                    string nomePasso = passo.Nome.Trim();

                    if (nomePasso == nomeOrigem) break;

                    if (anteriores.ContainsKey(nomePasso))
                        passo = anteriores[nomePasso];
                    else
                        passo = null;
                }

                caminho.Reverse(); // inverte para ficar Origem -> Destino

                int distanciaAcumulada = 0;

                // grid e a lista p o Mapa
                for (int i = 0; i < caminho.Count - 1; i++)
                {
                    Cidade atual = caminho[i];
                    Cidade proxima = caminho[i + 1];

                    int distTrecho = 0;

                    atual.Ligacoes.IniciarPercursoSequencial();
                    while (atual.Ligacoes.PodePercorrer())
                    {
                        if (atual.Ligacoes.Atual.Info.Destino.Trim() == proxima.Nome.Trim())
                        {
                            distTrecho = atual.Ligacoes.Atual.Info.Distancia;
                            break;
                        }
                    }

                    dgvRotas.Rows.Add(atual.Nome.Trim(), distTrecho);
                    distanciaAcumulada += distTrecho;

                    rotaEncontrada.Add(atual.Nome.Trim());
                }

                rotaEncontrada.Add(cidadeDestino.Nome.Trim());

                lbDistanciaTotal.Text = "Distância total: " + distanciaAcumulada + " km";

                pbMapa.Invalidate();

                MessageBox.Show($"Caminho encontrado com sucesso!\nDistância total: {distanciaAcumulada} km");
            }
            else
            {
                MessageBox.Show("Não foi encontrado nenhum caminho entre estas cidades.");
                lbDistanciaTotal.Text = "Distância total: ---";
            }
        }
        
        private void btnExcluirCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null) return;

            if (dgvLigacoes.CurrentRow == null)
            {
                MessageBox.Show("Selecione a linha da estrada para excluir.");
                return;
            }

            try
            {
                // pega os dados do Grid
                string nomeDestino = dgvLigacoes.CurrentRow.Cells[0].Value.ToString().Trim();
                int distancia = Convert.ToInt32(dgvLigacoes.CurrentRow.Cells[1].Value);

                string nomeOrigem = cidadeAtual.Nome.Trim();

                // remove da origem (ida)
                Ligacao ligIda = new Ligacao(nomeOrigem, nomeDestino, distancia);
                RemoverLigacaoDaLista(cidadeAtual.Ligacoes, ligIda);

                // remove do destino (volta)
                Cidade destinoChave = new Cidade(nomeDestino);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(destinoChave))
                {
                    Cidade destino = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;
                    Ligacao ligVolta = new Ligacao(nomeDestino, nomeOrigem, distancia);

                    RemoverLigacaoDaLista(destino.Ligacoes, ligVolta);
                }

                AtualizarGridLigacoesCidadeAtual();
                pbMapa.Invalidate();
                pnlArvore.Refresh();

                MessageBox.Show("Caminho excluído com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir: " + ex.Message);
            }
        }


        // atualizacoes

        private void AtualizarGridLigacoesCidadeAtual()
        {
            dgvLigacoes.Rows.Clear();

            if (cidadeAtual == null)
                return;

            foreach (Ligacao ligacao in cidadeAtual.Ligacoes.Listar())
            {
                dgvLigacoes.Rows.Add(ligacao.Destino.Trim(), ligacao.Distancia);
            }

            if (dgvLigacoes.Rows.Count > 0)
            {
                dgvLigacoes.CurrentCell = dgvLigacoes.Rows[0].Cells[0];
            }
        }

        private void AtualizarCombos()
        {
            cbxCidadeDestino.Items.Clear();

            List<Cidade> lista = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(lista);

            foreach (var item in lista)
            {
                if (!item.Excluido)
                {
                    cbxCidadeDestino.Items.Add(item.Nome.Trim());
                }
            }
        }


        // mapa

        // converte coordenadas normalizadas (0..1) em pixels do pb
        private PointF MapearCoordenada(double x, double y)
        {
            float posX = (float)(x * pbMapa.Width);
            float posY = (float)(y * pbMapa.Height);
            return new PointF(posX, posY);
        }

        // clique no mapa: define x e y normalizados para incluir/alterar cidades

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            // desenha o mapa de fundo
            if (mapa != null)
            {
                e.Graphics.DrawImage(mapa, 0, 0, pbMapa.Width, pbMapa.Height);
            }

            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            Dictionary<string, PointF> posicoes = new Dictionary<string, PointF>();

            // guarda posição das cidades
            foreach (Cidade c in todasCidades.Where(c => !c.Excluido))
            {
                posicoes[c.Nome] = MapearCoordenada(c.X, c.Y);
            }

            // desenha ligações como traços entre os pontos
            using (Pen penLigacao = new Pen(Color.LightGray, 1))
            {
                foreach (Cidade c in todasCidades)
                {
                    if (!c.Excluido)
                    {
                        if (!posicoes.ContainsKey(c.Nome))
                            continue;

                        PointF origem = posicoes[c.Nome];

                        c.Ligacoes.IniciarPercursoSequencial();
                        while (c.Ligacoes.PodePercorrer())
                        {
                            Ligacao lig = c.Ligacoes.Atual.Info;
                            string nomeDestino = lig.Destino.Trim();

                            if (!posicoes.ContainsKey(nomeDestino))
                                continue;

                            // desenha cada aresta só uma vez p nao duplicar traços
                            if (string.Compare(c.Nome, nomeDestino, StringComparison.Ordinal) < 0)
                            {
                                PointF destino = posicoes[nomeDestino];
                                e.Graphics.DrawLine(penLigacao, origem, destino);
                            }
                        }
                    }
                }
            }

            // se tiver rotaEncontrada (no futuro), podemos destacar em outra cor
            if (rotaEncontrada.Count > 1)
            {
                using (Pen penRota = new Pen(Color.Red, 3))
                {
                    for (int i = 0; i < rotaEncontrada.Count - 1; i++)
                    {
                        string nomeA = rotaEncontrada[i];
                        string nomeB = rotaEncontrada[i + 1];

                        if (posicoes.ContainsKey(nomeA) && posicoes.ContainsKey(nomeB))
                        {
                            e.Graphics.DrawLine(penRota, posicoes[nomeA], posicoes[nomeB]);
                        }
                    }
                }
            }

            // desenha cidades (vértices)
            float raio = 5f;
            using (Font fonte = new Font("Arial", 8))
            {
                foreach (Cidade c in todasCidades)
                {
                    if (!c.Excluido)
                    {
                        if (!posicoes.ContainsKey(c.Nome))
                            continue;

                        PointF pos = posicoes[c.Nome];
                        Brush brush = Brushes.Blue;

                        if (cidadeAtual != null && c.Nome == cidadeAtual.Nome)
                            brush = Brushes.Red;

                        if (rotaEncontrada.Contains(c.Nome))
                            brush = Brushes.Green;

                        e.Graphics.FillEllipse(brush, pos.X - raio, pos.Y - raio, 2 * raio, 2 * raio);
                        e.Graphics.DrawString(c.Nome.Trim(), fonte, Brushes.Black, pos.X + raio, pos.Y - raio);
                    }
                }
            }
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            arvoreBuscaBinariaBalanceadaAVL.Desenhar(pnlArvore);
        }

       
        // fechamento

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            try
            {
                // grava cidades e ligações usando nosso próprio método
                GravarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao salvar os dados: {ex.Message}\n" +
                    "O programa será encerrado sem salvar as últimas alterações.",
                    "Erro de Salvamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

    }
}
