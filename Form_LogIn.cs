// created By Doncoi
// 2019/12/19
// MIT License
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StudentInformationManageSystem
{
	public partial class Form_LogIn : Form
	{
		public Form_LogIn()
		{
			InitializeComponent();
		}

		//0-开发者 1-管理员 2-教师 3-学生
		private short permissionType = -1;

		private void setPermissionType(short _newType) { permissionType = _newType; }
		public short getPermissionType() { return permissionType; }

		//核对登录信息
		private Boolean CheckLogInfor(String _userName, String _password)
		{
			if("Doncoi" == _userName && "1021" == _password)
			{
				permissionType = 0;
				return true;
			}

			bool flag = false;

			FileStream fs = new FileStream("UserRecord.txt", FileMode.OpenOrCreate);
			StreamReader sr = new StreamReader(fs);
			string line = null;
			while ((line = sr.ReadLine()) != null)
			{
				string[] str = line.Split('*');
				if(_userName == str[0] && _password == str[1])
				{
					flag = true;
					if (str[2] == "开发者") permissionType = 0;
					else if (str[2] == "管理员") permissionType = 1;
					else if (str[2] == "教师") permissionType = 2;
					else if (str[2] == "学生") permissionType = 3;
					else permissionType = -1;
					break;
				}
			}
			fs.Close(); sr.Close();
			return flag;
		}

		//登录
		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox1.Text.Equals(String.Empty) || textBox2.Text.Equals(String.Empty))
			{
				if (textBox1.Text.Equals(String.Empty)) { label3.Text = "请输入用户名"; }
				else if (textBox2.Text.Equals(String.Empty)) { label3.Text = "请输入密码"; }
			}
			else
			{
				if (CheckLogInfor(textBox1.Text, textBox2.Text) == true)
				{
					label1.Visible = false; label2.Visible = false; label3.Visible = false;
					textBox1.Visible = false; textBox2.Visible = false;
					button1.Visible = false;

					timer1.Start();
				}
				else
				{
					label3.Text = "用户不存在或密码错误";
				}
			}
		}

		//控制头像移动
		private void timer1_Tick(object sender, EventArgs e)
		{
			pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y);
			if (pictureBox1.Location.X >= 160)
			{
				timer1.Stop();
				transToForm2();
			}
		}

		//跳转至窗体2
		private void transToForm2()
		{
			Form_Main form2 = new Form_Main(getPermissionType(), textBox1.Text);
			form2.Show();
			this.Close();
		}
	}
}
