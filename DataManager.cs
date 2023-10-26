using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingCarManager
{
    public class DataManager
    {
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
