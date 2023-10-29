using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ParkingCarManager
{
    public class DataManager
    {
        public static List<ParkingCar> Cars = new List<ParkingCar>();

        static DataManager()
        { 
            Load();
        }

        public static void Load()
        {
            try
            {
                DBHelper.selectQuery(); //전체 조회
                Cars.Clear(); // list비우기
                foreach(DataRow item in DBHelper.dt.Rows)
                {
                    ParkingCar car = new ParkingCar();
                    car.ParkingSpot = item["parkingSpot"].ToString();
                    car.carNumber = item["carNumber"].ToString();
                    car.driverName = item["driverName"].ToString();
                    car.phoneNumber = item["phoneNumber"].ToString();
                    car.parkingTime = item["parkingTime"].ToString() == "" ?
                        new DateTime() : DateTime.Parse(item["parkingTime"].ToString());
                    Cars.Add(car);
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                printLog(e.StackTrace + "load");
            }
        }
        // 주차 출차용 Save(false가 주차, true가 출차)
        public static void Save(string ps, string carNumber, string driverName, string phoneNumber, bool isRemove=false)
        {
            try
            {
                DBHelper.updateQuery(ps, carNumber, driverName, phoneNumber, isRemove);
            }
            catch (Exception)
            {

            }
        }
        // 주차 공간 추가 삭제용 Save
        public static bool Save(string command, string ps, out string contents)
        {
            DBHelper.selectQuery(ps); // 해당 공간 유무 체크
            contents = "";
            if (command.Equals("insert"))
                return DBInsert(ps, ref contents);
            else
                return DBDelete(ps, ref contents);
        }

        private static bool DBInsert(string ps, ref string contents)
        {
            if(DBHelper.dt.Rows.Count==0) // 공간이 하나도 없는 경우
            {
                DBHelper.insertQuery(ps);
                contents = $"주차공간 {ps}이/가 추가되었습니다.";
                return true;
            }
            else
            {
                contents = $"주차 공간이 이미 찼습니다.";
                return false;
            }
        }

        private static bool DBDelete(string ps, ref string contents)
        {
            if (DBHelper.dt.Rows.Count == 0) // 공간이 하나도 없는 경우
            {
                DBHelper.insertQuery(ps);
                contents = $"주차공간 {ps}이/가 삭제되었습니다.";
                return true;
            }
            else
            {
                contents = $"주차할 공간이 없습니다.";
                return false;
            }
        }

        public static void printLog(string contents)
        {
            //프로젝트명.exe와 같은 경로에 LodFolder가 없다면
            DirectoryInfo di = new DirectoryInfo("LogFolder");
            if (di.Exists == false)
            {
                di.Create();
            }
            using (StreamWriter w = new StreamWriter
                (@"LogFolder\History.txt", true))
            {
                w.WriteLine(contents);
            }
        }
    }
}
