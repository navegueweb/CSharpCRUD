using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecology.DataBase {

    /// <summary>
    /// Esta classe implementa a operação de Read das quatro operações básicas de CRUD.
    /// </summary>
	partial class CRUD : Connection {
        
        // Recebe a quantidade limite de campso a serem exibidos
        private int intLimitStart = 0;
		private int intLimitEnd = 0;

		#region Limitação de exibição de registros (LIMIT/TOP)
		public void Limit(int start = 0, int end = 0){
			intLimitStart = start;
			intLimitEnd = end;
		}
		public void Top(int start = 0, int end = 0) {
			intLimitStart = start;
			intLimitEnd = end;
		}
        #endregion

        // Corresponde as colunas que serão selecionadas / exibidas
        List<string> selectCollumn = new List<string>();
        public void Select(string collumn) {
            if(selectCollumn.Contains(collumn)) this.selectCollumn.Remove(collumn);
			this.selectCollumn.Add(collumn);
		}

        // Monta o argumento correspondente ao BETWEEN
        string between = null;
        public void Between(string collumn, string valueStart, string valueEnd) {
            between = collumn + " BETWEEN '" + valueStart + "' AND '" + valueEnd + "' ";
        }

        string orderBy = null;
        public void OrderBy(string by = null, string order = "ASC") {
            this.orderBy = " ORDER BY [" + by + "] " + order;
        }
		
        /* Implementa a leitura do banco de dados */
        public DataTable Get(string table = null){
            if (table != null) {
                this.Table = table;
            };
            
			// Inicia a montagem do SQL para consulta no banco de dados
			this.LastSQL = "SELECT ";

			// Verifica se há quantidade limite de valores a serem exibidos
			if(intLimitStart > 0){
				this.LastSQL += " TOP ";

				// Verifica se existe quantidade inicial e final para exibição
				this.LastSQL += intLimitStart > 0 ? (intLimitStart.ToString() + " ") : "";
				this.LastSQL += (intLimitStart > 0 && intLimitEnd > 0) ? (", " + intLimitEnd.ToString() + " ") : "";
			}

            // Verifica quais as colunas que devem ser selecionadas.
            int totalCollumn = 1;
            if (selectCollumn.Count > 0) {
                selectCollumn.ForEach(delegate (string key) {
                    this.LastSQL += " [" + key + "] " + (totalCollumn >= selectCollumn.Count ? " " : ", ");
                    totalCollumn++;
                });
            } else {
                this.LastSQL += " * "; /* Seleciona todas as colunas da tabela */
            };

            // Monta o argumento FROM
            this.LastSQL += " FROM [" + Table + "] ";

            if (strQuery != null) this.LastSQL = strQuery;

			// Inicia a montagem dos parametros WHERE
			if(whereData.Count > 0 | whereLike.Count > 0 | listWhereData.Count > 0 | between != null) {
				this.LastSQL += " WHERE ";
                
                if (whereData.Count > 0) {
                	int intTotalCollumWhere = 1;
                	foreach (string keyWhere in whereData.Keys) {
                		this.LastSQL += " [" + keyWhere + "] = '" + whereData[keyWhere] + (intTotalCollumWhere >= whereData.Count ? "' " : "' AND ");
                		intTotalCollumWhere++;
                	};
                };

                // Monta o argumento WHERE com metodo subescrito com parâmetro de operador de comparação.
                if (listWhereData.Count > 0) {
                    if (whereData.Count > 0) { this.LastSQL += " AND "; };

                    int totalListWhereData = 1;
                    if (listWhereData.Count > 0) {
                        listWhereData.ForEach(delegate (string key) {
                            this.LastSQL += listWhereData + (totalListWhereData >= listWhereData.Count ? "' " : "' AND "); ;
                            totalListWhereData++;
                        });
                    }
                };

                // Monta o argumento LIKE
                if (whereLike.Count > 0) {
					if(whereData.Count > 0) { this.LastSQL += " AND "; };

					int intTotalCollumWhereLike = 1;
					foreach (string keyWhere in whereLike.Keys) {
						this.LastSQL += " [" + keyWhere + "] = '" + whereLike[keyWhere] + (intTotalCollumWhereLike >= whereLike.Count ? "' " : "' AND ");
						intTotalCollumWhereLike++;
					};
				};

                // Monta o argumento BETWEEN
                if (between != null) {
                    if (whereData.Count > 0) { this.LastSQL += " AND "; };
                    this.LastSQL += between;
                }
            };
            
			// Monta o argumento ORDER BY
			if (this.orderBy != null) {
                this.LastSQL += this.orderBy;
            };

            // Executa a consulta e retorna um DataTable
            DataTable dt = null;
            
            try {
                this.DbOpen();
                SqlCeCommand command = new SqlCeCommand(this.LastSQL, this.connection);
                SqlCeDataReader dr = command.ExecuteReader();

                dt = new DataTable();
                dt.Load(dr);

                logs.Create("Último SQL rodado: " + this.LastSQL, "DEBUG");

                this.DbClose();
            } catch (Exception error) {
                logs.Create("Último SQL rodado: " + this.LastSQL, "DEBUG");
                logs.Create("Erro ao consultar em " + this.Table + ". Erro informado: " + error.Message, "ERROR");
                this.Errors = error.Message;

                this.DbClose();
            }

            return dt;
        }

	}
}
