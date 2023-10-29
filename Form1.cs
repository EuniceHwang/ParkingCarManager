using System;
using System.Linq;
using System.Windows.Forms;

namespace ParkingCarManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = DateTime.Now.ToString("yyyy년 MM월 dd일 HH mm분 ss초");
            timer1.Start();
            try
            {
                textBox1.Text = DataManager.Cars[0].ParkingSpot;
                textBox2.Text = DataManager.Cars[0].carNumber;
                textBox3.Text = DataManager.Cars[0].driverName;
                textBox4.Text = DataManager.Cars[0].phoneNumber;
                textBox5.Text = textBox1.Text;

            }
            catch (Exception ex)
            {
                
            }
            if(DataManager.Cars.Count>0)
                dataGridView1.DataSource = DataManager.Cars;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분 ss초");
        }

        private void WriteLog(string contents)
        {
            string log =
                $"[{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff")}]";
            log += contents;
            DataManager.printLog(log);
            listBox1.Items.Insert(0, log);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Trim() == "")
            {
                MessageBox.Show("주차공간 번호 입력하세요(주차).");
            }
            else if(textBox2.Text.Trim()=="")
            {
                MessageBox.Show("차량 번호 입력해주세요.");
            }
            else
            {
                try
                {
                    ParkingCar car = DataManager.Cars.Single(x => x.ParkingSpot.Equals(textBox1.Text));
                    if (car.carNumber.Trim() != "")
                    {
                        MessageBox.Show("해당 공간에 이미 차가 있습니다.");
                    }
                    else
                    {
                        car.carNumber = textBox2.Text;
                        car.driverName = textBox3.Text;
                        car.phoneNumber = textBox4.Text;
                        car.parkingTime = DateTime.Now;

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Cars;
                        // 주차용
                        DataManager.Save(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                        string contents = $"주차 공간 {textBox1.Text}에 " + $"{textBox2.Text}차를 주차했습니다.";
                        WriteLog(contents);
                        MessageBox.Show(contents);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"주차공간 {textBox1.Text} 없습니다.");
                    WriteLog($"주차공간 {textBox1.Text} 없습니다.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("주차공간 번호 입력하세요(출차).");
            }
            else
            {
                try
                {
                    ParkingCar car = DataManager.Cars.Single(x => x.ParkingSpot.Equals(textBox1.Text));
                    if (car.carNumber.Trim() == "")
                    {
                        MessageBox.Show("해당 공간이 비었습니다.");
                    }
                    else
                    {
                        string oldCar = car.carNumber;
                        car.carNumber = "";
                        car.driverName = "";
                        car.phoneNumber = "";
                        car.parkingTime = new DateTime();

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Cars;
                        // 출차용
                        DataManager.Save(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, true);
                        string contents = $"주차 공간 {textBox1.Text}에 " + $"{oldCar}차를 주차했습니다.";
                        WriteLog(contents);
                        MessageBox.Show(contents);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"주차공간 {textBox1.Text} 없습니다.");
                    WriteLog($"주차공간 {textBox1.Text} 없습니다.");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ParkingCar car = dataGridView1.CurrentRow.DataBoundItem as ParkingCar;
            textBox1.Text = car.ParkingSpot;
            textBox2.Text = car.carNumber;
            textBox3.Text = car.driverName;
            textBox4.Text = car.phoneNumber;
            textBox5.Text = textBox1.Text;
        }

        private string lookUpParkingSpot(string ps)
        {
            string parkedCarNum = "";
            foreach(var item in DataManager.Cars)
            {
                if(item.ParkingSpot.Equals(ps))
                {
                    parkedCarNum = item.carNumber;
                    break;
                }
            }
            return parkedCarNum;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ps = textBox5.Text;
            string parkingCar = lookUpParkingSpot(ps);
            string contents = "";
            if (parkingCar.Trim() != "")
            {
                contents = $"주차공간 {ps}에 주차된 차는 {parkingCar}입니다."
            }
            else
                contents = $"주차공간 {ps}에 차가 없습니다.";
            WriteLog(contents);
            MessageBox.Show(contents);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            spot_add_delete(textBox5.Text, "insert");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            spot_add_delete(textBox5.Text, "delete");
        }

        private void spot_add_delete(string text, string v)
        {
            text = text.Trim(); // 좌우 공백 삭제
            string contents = "";
            bool check = DataManager.Save(v, text, out contents);
            if(check)
            {
                button6.PerformClick(); // Refresh 버튼 클릭(=전체조회)
            }
            MessageBox.Show(contents);
            WriteLog(contents);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataManager.Load(); // 전체조회(다시 불러오기)
            dataGridView1.DataSource = null;
            if(DataManager.Cars.Count>0)
            {
                dataGridView1.DataSource= DataManager.Cars;
            }
        }
    }
}
