using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoBot.Data;
using EchoBot.DatabaseAccess;

namespace EchoBot.Data
{
    public class GetWebinarTermine
    {
        public List<TerminData> GetTermineFromSql()
        {
            DatabaseConnection connection = new DatabaseConnection();
            var sqlConnection = connection.OpenSqlConnection();

            List<TerminData> termine = WebinarTermine(sqlConnection);

            connection.CloseSqlConnection();

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
    }
}
