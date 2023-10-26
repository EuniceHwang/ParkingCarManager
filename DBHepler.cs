using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingCarManager
{
    public class DBHepler
    {
        private static SqlConnection conn = new SqlConnection();
        public static SqlDataAdapter da;
        public static DataSet ds;
        public static DataTable dt;
        private static void ConnectDB()
        {
            string dataSource = "local";
            string db = "MYDB";
            string security = "SSPI";
            conn.ConnectionString =
                $"Data Source = ({dataSource}); initial Catalog = {db};" +
                $"integrated Security = {security};" +
                $"Timeout=3";
            conn = new SqlConnection(conn.ConnectionString);
            conn.Open();
        }

        public static void selectQuery(string ps="-1")
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (ps.Equals("-1"))
                    cmd.CommandText = "select * from parkingManager";
                else
                    cmd.CommandText = $"select * from parkingManager " +
                        $"where pakringSpot='{ps}'";
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds, "ParkingManager");
                dt = ds.Tables[0];

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("select 오류");
                DataManager.printLog("select," + ex.Message + "," + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void updateQuery
        (string parkingSpot, string carNumber, string driverName, string phoneNumber, bool isRemove)
        {
            try
            {
                ConnectDB();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                string sqlcommand = "";
                if (isRemove) //출차
                {
                    //sql injection 방지 코드
                    sqlcommand = "update parkingManger set carNumber=''," +
                    "driverName='', phoneNumber=''," +
                    "parkingTime=null where" +
                    "parkingSpot=@p1";
                    cmd.Parameters.AddWithValue("@p1", parkingSpot);
                }
                else //주차
                {
                    //sql injection 방지 코드
                    sqlcommand = "update parkingManger set carNumber=@p1," +
                    "driverName=@p2, phoneNumber=@p3," +
                    "parkingTime=@p4 where" +
                    "parkingSpot=@p5";
                    cmd.Parameters.AddWithValue("@p1", carNumber);
                    cmd.Parameters.AddWithValue("@p2", driverName);
                    cmd.Parameters.AddWithValue("@p3", phoneNumber);
                    cmd.Parameters.AddWithValue("@p4", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    cmd.Parameters.AddWithValue("@p5", parkingSpot);
                }
                cmd.CommandText = sqlcommand;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("update" + ex.Message);
                DataManager.printLog("update," + ex.Message + "," + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        private static void executeQuery(string ps, string command)
        {

        }

        public static void deleteQuery(string ps)
        {
            executeQuery(ps, "delete");
        }

        public static void insertQuery(string ps)
        {
            executeQuery(ps, "insert");
        }

    }
}
