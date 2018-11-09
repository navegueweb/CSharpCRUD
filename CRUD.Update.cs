using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecology.DataBase {
	/// <summary>
	/// Esta classe parcial é responsável por escrever a operação de Update do conceito de crUd.
	/// </summary>
	partial class CRUD : Connection{

		public bool Update(string table = null){
			bool updateResult = false;

			if(table != null){
				this.Table = table;
			};

			// Começa a criação do SQL
			this.LastSQL = "UPDATE [" + this.Table + "] SET ";

			// Monta os parâmetros SET
			if (dataSet.Count > 0) {
				int intTotalCollumWhere = 1;
				foreach (string key in dataSet.Keys) {
					this.LastSQL += " [" + key + "] = '" + dataSet[key] + (intTotalCollumWhere >= dataSet.Count ? "' " : "', ");
					intTotalCollumWhere++;
				};
			};

			// Começa a montar o bloco WHERE
			this.LastSQL += " WHERE ";
			if (whereData.Count > 0) {
				
				int intTotalCollumWhere = 1;
				foreach (string key in whereData.Keys) {
					this.LastSQL += " [" + key + "] = '" + whereData[key] + (intTotalCollumWhere >= whereData.Count ? "' " : "' AND ");
					intTotalCollumWhere++;
				};
			};

            // Monta o argumento WHERE com metodo subescrito com parâmetro de operador de comparação.
            if (listWhereData.Count > 0) {
                if (whereData.Count > 0) { this.LastSQL = " AND "; };

                int totalListWhereData = 1;
                if (listWhereData.Count > 0) {
                    listWhereData.ForEach(delegate (string key) {
                        this.LastSQL += listWhereData + (totalListWhereData >= listWhereData.Count ? "' " : "' AND "); ;
                        totalListWhereData++;
                    });
                }
            };

            // Finaliza gravando as informações no banco de dados
            try {
				this.DbOpen();

				SqlCeCommand command = new SqlCeCommand(this.LastSQL, this.connection);
				int result = command.ExecuteNonQuery();

				this.DbClose();

                if (result > 0) {
                    return true;
                } else {
                    logs.Create("Último SQL rodado: " + this.LastSQL, "ERROR");
                    return false;
                };

			} catch (Exception Error) {
				logs.Create("Erro ao salvar dados do cliente. Mensagem retornada: " + Error.Message, "ERROR");
				logs.Create("Último SQL rodado: " + this.LastSQL, "ERROR");
				
				this.DbClose();
			}

			return updateResult;
		}

	}
}
