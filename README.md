# CSharpCRUD
Conjunto de classes inspirado no framework PHP Codeigniter para realizar um CRUD facilmente no banco de dados SQL Server CE.

Uma observação importante é que este crud foi desenvolvido no Visual Studio 2017 Community

## Classe CONNECTION
Realiza a conexão com o banco de dados buscando a string de conexão no arquivo de propriedades da aplicação. Essa classe é herdada automáticamente pelas outras classes. Se não houver situações especiais, não será preciso implementar nada dessa classe. Apenas da classe CRUD e suas demais parciais.

Métodos da classe:
- Connection() - Método construtor que configura a conexão com o BD.
- DBOpen() - Abre a conexão com o banco de dados.
- DBClose() - Fecha a conexão com o banco de dados.

Propriedades da classe:
- Errors - Recebe e devolve os erros do banco de dados
- LastSQL - Recebe e devolve os últimos comandos SQL executados.

## Classes CRUD
Parte principal desse conjunto. Possui alguns métodos que serão usados pelas outras demais. Herda o conteúdo da classe Connection.

(Ainda falta escrever a documentação)
