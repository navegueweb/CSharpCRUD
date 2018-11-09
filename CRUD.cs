using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecology.DataBase {
    partial class CRUD : Connection{

        // Recebe e recupera a o nome da tabela
        public string Table { get; set; }

        // Recebe e recupera a última chave primaria inserida
        public int LastPrimaryKey = 0;
		
		// Limpa as instâncias já criadas
		public void Clear() {
			whereData.Clear();
			dataSet.Clear();
			whereLike.Clear();
			selectCollumn.Clear();
			Table = null;
        }

        private string strQuery = null;
        public void Query(string query) {
            strQuery = query;
        }

        // Implementa a passagem de dados a serem salvos via INSERT ou UPDATE
        Dictionary<string, string> dataSet = new Dictionary<string, string>();
        public void Set(string key, string value) {
            if (dataSet.ContainsKey(key)) {
                dataSet.Remove(key);
            };
            dataSet.Add(key, value);
        }

        // Corresponde aos parâmetros WHERE
        Dictionary<string, string> whereData = new Dictionary<string, string>();
        public void Where(string collumn, string collumnValue) {
            if ( whereData.ContainsKey(collumn) ){
			    whereData.Remove(collumn);
			};
			whereData.Add(collumn, collumnValue);
		}

        // Corresponde aos parametros WHERE
        List<string> listWhereData = new List<string>();
        public void Where(string collumn, string collumnValue, string comparisionOperator) {
            // Monta o parametro 
            string strWhere = " [" + collumn + "]" + comparisionOperator + "'" + collumnValue + "' ";

            if (listWhereData.Contains(strWhere)) {
                listWhereData.Remove(strWhere);
            };
            listWhereData.Add(strWhere);
        }
        
        /* Corresponde ao tipo de consulta LIKE */
        Dictionary<string, string> whereLike = new Dictionary<string, string>();
		public void Like(string collumn, string value) {
			if (whereLike.ContainsKey(collumn)) {
				whereLike.Remove(collumn);
			};

			whereLike.Add(collumn, value);
		}
    }
}
