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
            var databaseConnection = new DatabaseConnection();
            List<TerminData> termine = databaseConnection.GetWebinarTermine();

            return termine;
        }     
    }
}