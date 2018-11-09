using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecology.DataBase {
	partial class CRUD : Connection{

		public bool Delete(string table = null){
			bool deleteResult = false;

			if( table != null ){
				this.Table = table;
			};

			// inicia a Query de deleção
			this.LastSQL = "DELETE FROM [" + this.Table + "] WHERE ";

			// Começa a montar o bloco WHERE
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

            try {
				this.DbOpen();
				SqlCeCommand command = new SqlCeCommand(this.LastSQL, this.connection);
				command.ExecuteNonQuery();
				this.DbClose();

				deleteResult = true;
			} catch (Exception Error) {
				logs.Create("Último SQL rodado: " + this.LastSQL, "DEBUG");
				logs.Create("Erro ao excluir registro de cliente. Mais detalhes: " + Error.Message, "ERROR");
				this.DbClose();
			}

			return deleteResult;
		}

	}
}
