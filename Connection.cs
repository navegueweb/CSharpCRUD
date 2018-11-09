using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecology.DataBase{
    class Connection {
        private string connectionString = Properties.Settings.Default.dbConnectionString;
		
		public SqlCeConnection connection;
        public Library.Logs logs = new Library.Logs();
        
        // Construtor, inicializa variáveis de conexão.
        public Connection() {
			
			try {
                connection = new SqlCeConnection(connectionString);
            } catch (Exception Error) {
                logs.Create("Não foi possível conectar ao banco de dados. Veja o erro informado: " + Error.Message, "ERROR");
            }
        }
        
        // Erros lançados durante a conexão com o banco de dados
        public string Errors { get; set; }

        // Última consulta SQL feita.
        public string LastSQL { get; set; }

        // Abre a conexão com o banco de dados
        public bool DbOpen() {
			
            if ( !File.Exists(Properties.Settings.Default.configLocalSaveDB + "database.sdf") ) {
                Errors = "Arquivo de banco de dados não encontrado.";
                logs.Create("Arquivo de banco de dados não encontrado.", "ERROR");
                return false;

            } else {
                try {
                    connection.Open();
                    return true;
                } catch (Exception Error) {
                    Errors = Error.Message;
                    logs.Create("Erro na conexão: " + Error.Message, "ERROR");
                    return false;
                }
            }
        }
        
        // Fecha a conexão com o banco de dados
        public bool DbClose() {
            try {
                connection.Close();
                return true;
            } catch (Exception Error) {
                Errors = Error.Message;
                logs.Create(Error.Message, "ERROR");
                return false;
            }
        }
    }
}
