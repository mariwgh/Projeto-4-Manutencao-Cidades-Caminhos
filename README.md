<div align="center">

# Manutenção de Cidades e Caminhos - Cadastro de cidades, de rotas e busca de caminhos  
*A Rede Estadual Rodoviária de São Paulo - deseja fornecer aos seus usuários um aplicativo que
permita verificar os caminhos entre cidades, através de viagens de ônibus.*

</div>

Deseja-se que o aplicativo efetue a manutenção do arquivo de cidades e do arquivo de caminhos
entre as cidades, bem como permita a busca de caminhos entre cidades através de ônibus.

<div></div>

O arquivo binário de cidades deve ser lido recursivamente e seus registros armazenados em uma
árvore de busca binária balanceada AVL, para fazer a manutenção dos dados de cidades. Em
cada nó dessa árvore de cidades, o registro de cidade armazenado deverá, também, possuir um
atributo da classe ListaSimples, inicialmente vazio. Depois da leitura do arquivo de cidades e da
montagem da árvore balanceada AVL, o arquivo de caminhos será lido e os caminhos que tem
origem em cada cidade serão armazenados na lista ligada encapsulada dentro do objeto Info de
cada nó de cidade, representando a lista de cidades acessíveis a partir da cidade de origem
armazenada no nó da árvore.

<div></div>

Após o arquivo de cidades ter sido lido e sua árvore montada, deve-se passar a ler o arquivo de
caminhos, sequencialmente. Cada registro de caminho deverá ser armazenado ao final da lista
ligada armazenada no nó da cidade de origem na árvore de cidades.
Como se observa na figura acima, em cada nó de árvore se origina uma lista de caminhos, que
representa todas as saídas da cidade representada pelo nó. Observe que a árvore é mantida 
balanceada logo depois da leitura recursiva dos registros do arquivo de cidades e a cada inclusão
ou exclusão, pois é uma árvore AVL.

<div></div>

Na inclusão de cidade, pressiona-se o botão [Incluir] e, a partir daí, o usuário deverá digitar o
nome da cidade que deseja incluir, verificar (evento Leave do textBox) se a cidade não existe. Não
existindo, deve-se clicar no local do mapa onde fica essa cidade para que suas coordenadas X e
Y proporcionais sejam preenchidas nos numericUpDowns udX e udY. Após isso, a cidade deve
ser incluída na árvore de busca balanceada AVL.
No botão [Buscar], deve-se mostrar os dados da cidade cujo nome foi digitado anteriormente no
txtNomeCIdade. Caso a cidade exista na árvore de busca, deve-se exibir suas coordenadas e as
cidades para as quais tem ligação.
Quando o botão [Alterar] for pressionado, deve-se alterar as coordenadas proporcionais X e Y da
cidade cujo nome foi buscado anteriormente e está exibida na tela, nesse momento. Não altere
nomes de cidades.
Quando o botão [Excluir] for alterado, deve-se marcar como excluida da árvore de busca a cidade
cujo nome foi buscado anteriormente e está exibida na tela, nesse momento. No entanto, caso
essa cidade possua ligação com alguma outra cidade, não poderá ser excluída.
Abaixo desses controles visuais, a guia terá um dataGridView onde serão exibidos os dados dos
caminhos originários (ligações) na cidade que está sendo apresentada na tela. Em cada linha
desse dataGridView teremos colunas para exibir o nome da cidade de destino e a distância até
ela.
Quando o botão [+] for pressionado, deve-se incluir, na lista ligada de cidades originárias da
cidade atual, um nó com o nome da cidade Novo destino e sua distância. Deve-se garantir que o
nome da cidade de destino exista também na árvore, e uma ligação desta cidade com a cidade
exibida deve também ser incluída na lista de cidades ligadas da cidade de destino, pois as
ligações são bidirecionais.
Quando o botão [-] for pressionado, deve-se excluir a ligação que corresponde à linha selecionada
no dataGridView dgvLigacoes. Lembre-se de excluir essa ligação na lista ligada de ligações da
cidade de origem e na cidade de destino, em seus respectivo nós da árvore de busca.

<div></div>

Do lado direito do formulário, há um PictureBox onde se desenhará o mapa do Estado de São
Paulo, no qual ainda deverão ser apresentados um pequeno círculo preenchido na localização
geográfica de cada cidade presente na árvore de busca, bem como escrever o nome da cidade
próximo a esse círculo. Abaixo temos um exemplo, não exatamente igual ao dos arquivos. É
interessante, também, exibir todas as ligações entre as cidades, sempre que se atualizar o mapa.

<div></div>

Numa outra guia do TabControl (tpArvore), há um Panel interno onde a árvore AVL será
desenhada, mostrando o nome de cada cidade nos nós exibidos e a quantidade de caminhos que
ela possui, não sendo esperado que se desenhe a lista ligada originária de cada nó da árvore.
Obviamente, o mapa no seu tamanho original não caberá na tela. Portanto, permita que o mapa
seja armazenado num componente PictureBox que se ajuste ao tamanho da tela e lembre-se que
isso mudará as coordenadas de exibição de cada cidade no mapa proporcionalmente à
mudança da altura y e largura x do mapa apresentado na tela, numa proporção entre a largura e a
altura da tela com a coordenada (X, Y) original da cidade.

<div></div>

No evento Paint do PictureBox - exibir os nomes e locais das cidades no mapa, de acordo com a
proporção entre coordenadas das cidades referentes ao tamanho original (2560 x 1600) e as
dimensões atuais do picturebox.
Quando o usuário desejar buscar uma rota de ônibus entre a cidade atualmente exibida e uma
cidade de destino qualquer, deverá selecionar a cidade de destino no combobox
cbxCidadeDestino e pressionar o botão [Buscar Caminho]. Nesse momente, deverá ser usado o
algoritmo de Dijkstra para determinar um caminho entre as cidades, avisando caso não exista e
exibindo cada vértice da rota determinada pelo algoritmo caso um caminho seja encontrado.
Também deve-se desenhar as linhas retas ligando as cidades que fazem parte dessa rota.

<div></div>

Quando terminar a execução do programa, a árvore de busca deverá ser percorrida em ordem, e
seus registros gravados no arquivo binário de cidades em ordem crescente. Como cada nó
possui uma lista ligada de caminhos, os respectivos registros armazenados em cada uma dessas
listas deverão ser gravados no arquivo texto de caminhos.
