using TeamsBot.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsBot.DatabaseAccess
{
    public class DatabaseConnection
    {

        public List<TerminData> GetTermineFromSql()
        {
            var sqlConnection = OpenSqlConnection();
            List<TerminData> termine = WebinarTermine(sqlConnection);
            CloseSqlConnection(sqlConnection);

            return termine;
        }


        public List<TerminData> WebinarTermine(SqlConnection sqlConnection)
        {
            string terminAbfrageString = "Select ID, Datum, StartZeit, EndZeit from dbo.WebinarTermine";


            SqlCommand command = new SqlCommand(terminAbfrageString, sqlConnection);
            using (SqlDataReader terminReader = command.ExecuteReader())
            {
                StringBuilder builder = new StringBuilder();
                var terminList = new List<TerminData>();
                while (terminReader.Read())
                {
                    terminList.Add(new TerminData { ID = terminReader.GetInt32(0), Datum = terminReader.GetDateTime(1), Startzeit = terminReader.GetTimeSpan(2), Endzeit = terminReader.GetTimeSpan(3) });
                }
                terminReader.Close();
                return terminList;
            }
        }



        private SqlConnection OpenSqlConnection()
        {
            string sqlConnectionString = "Server=tcp:webinarazuresqldb.database.windows.net,1433;Initial Catalog=WebinarDB;Persist Security Info=False;" +
             "User ID=benschaller;Password=Admin1234!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection sqlConnection = new SqlConnection(sqlConnectionString);

            sqlConnection.Open();
            return sqlConnection;
        }

        private void CloseSqlConnection(SqlConnection sqlConnection)
        {
            sqlConnection.Close();
        }

    }
}
