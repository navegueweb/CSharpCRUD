using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecology.DataBase {
    /// <summary>
    /// Esta classe implementa a operação Create 
    /// </summary>
    partial class CRUD : Connection{

        public bool Insert(string table, string primaryKey) {
			bool insertResult = false;

            if (table != null) {
                this.Table = table;
            };

            // Começa a montagem da Query
            this.LastSQL = "INSERT INTO [" + this.Table + "] (";

            // Seta as colunas que serão inseridos
            int intTotalCollumWhere = 1;
            foreach (string keyWhere in dataSet.Keys) {
                this.LastSQL += "[" + keyWhere + "]" + (intTotalCollumWhere >= dataSet.Count ? " " : ",  ");
                intTotalCollumWhere++;
            };

            // Seta os valores que serão incluídos nas colunas do registro
            this.LastSQL += ") VALUES (";
            intTotalCollumWhere = 1;
            foreach (string valWhere in dataSet.Values) {
                this.LastSQL += " '" + valWhere + (intTotalCollumWhere >= dataSet.Count ? "' " : "',  ");
                intTotalCollumWhere++;
            };

            this.LastSQL += ")";

			try {
				this.DbOpen();

				SqlCeCommand command = new SqlCeCommand(this.LastSQL, this.connection);
				command.ExecuteNonQuery();

				this.DbClose();

                CRUD dbCrud = new CRUD();
                dbCrud.Clear();
                dbCrud.Top(1);
                dbCrud.Select(primaryKey);
                dbCrud.OrderBy(primaryKey, "DESC");
                DataTable dt = dbCrud.Get(table);
				
				if (dt != null) {
					insertResult = true;
					this.LastPrimaryKey = Convert.ToInt32(dt.Rows[0][primaryKey].ToString());
				} else {
					insertResult = false;
					this.LastPrimaryKey = 0;
				};

			} catch (Exception Error) {
				logs.Create("Erro ao salvar dados em " + this.Table + ". Mensagem retornada: " + Error.Message, "ERROR");
				logs.Create("Último SQL rodado: " + this.LastSQL, "ERROR");
				this.DbClose();
			}

			return insertResult;
		}
    }
}
