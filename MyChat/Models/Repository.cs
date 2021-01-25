using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyChat.DataModels;


namespace MyChat.Models
{
    public class Repository
    {


        SqlConnection con = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        public List<tbl_messages> GetMessages(string userCode, string receiverCode)
        {
            List<tbl_messages> messages = new List<tbl_messages>();
            using (var cmd = new SqlCommand("select MessageCode, Id, ChatText, isDelete, isDeleteSender, isDeleteReceiver from dbo.tbl_messages where IsDelete<>'Y' and (IsDeleteSender<>'Y' or IsDeleteReceiver<>'Y') and ( (sender='" + userCode + "' and receiver='" + receiverCode + "' ) or  (sender='" + receiverCode + "' and receiver='" + userCode + "' ) )", con))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.Notification = null;


                var dependency = new SqlDependency(cmd);
                dependency.OnChange += dependency_OnChange;


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                con.Dispose();
            }
            return messages;

        }
        
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;
            dependency.OnChange -= dependency_OnChange;

            if (e.Type == SqlNotificationType.Change)
            {
                Message.SendMessages();
            }


        }


    }
}