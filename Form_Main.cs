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
	public partial class Form_Main : Form
	{
		//0-开发者 1-管理员 2-教师 3-学生
		//在进入窗体时唯一更改并锁定
		private readonly short currentPermissionType = -1;
		//当前用户名
		//在进入窗体时唯一更改并锁定
		private readonly String currentUserName;
		//1-信息管理模式 2-账户管理模式 3-学生选课模式
		//随用户操作多次更新
		private short oprMode;
		//学生选课信息
		private int TotalCourseSelectedCount;
		private int TotalCredits;
		//主窗体初始化
		public Form_Main(short _currentPermissionType, String _currentUserName)
		{
			currentPermissionType = _currentPermissionType;
			currentUserName = _currentUserName;
			oprMode = 1;

			InitializeComponent();
			MainFormLoad();
		}
		/////////////////////////////////////////////////////////////////////////////
		//操作模式和数据更新
		/////////////////////////////////////////////////////////////////////////////
		//保存当前表单
		private void saveListView()
		{
			//保存学生信息
			StreamWriter sw = new StreamWriter("StudentRecord.txt", false, Encoding.UTF8);
			foreach (ListViewItem lvi in listView1.Items)
			{
				string str = "";
				for (int i = 0; i < listView1.Columns.Count; ++i)
					str += lvi.SubItems[i].Text + "*";
				sw.WriteLine(str);
			}
			sw.Close();
			//保存账户信息
			sw = new StreamWriter("UserRecord.txt", false, Encoding.UTF8);
			foreach (ListViewItem lvi in listView2.Items)
			{
				string str = "";
				for (int i = 0; i < listView2.Columns.Count; ++i)
					str += lvi.SubItems[i].Text + "*";
				sw.WriteLine(str);
			}
			sw.Close();
			//保存选课信息 仅当账户为学生
			if (3 == currentPermissionType)
			{
				FileStream fs = new FileStream("CourseRecord.txt", FileMode.OpenOrCreate);
				StreamReader sr = new StreamReader(fs);
				string line = null; int temp = 0;
				while ((line = sr.ReadLine()) != null)
				{
					string[] str = line.Split('*');
					//如果找到与当前用户匹配的记录
					if (str[0] == currentUserName)
					{
						//去除原有信息
						var strs = System.IO.File.ReadAllLines("CourseRecord.txt", Encoding.UTF8).ToList();
						strs.RemoveAt(temp + 1);
						System.IO.File.WriteAllLines("CourseRecord.txt", strs, Encoding.UTF8);
					}
					temp++;
				}
				fs.Close(); sr.Close();

				line = currentUserName + "*";
				sw = new StreamWriter("CourseRecord.txt", true, Encoding.UTF8);
				foreach (ListViewItem lvi in listView4.Items)
				{
					for (int i = 0; i < listView4.Columns.Count; ++i)
						line += lvi.SubItems[i].Text + "*";
				}
				sw.WriteLine(line);
				sw.Close();
			}

			//更新状态栏
			if (label4.Text.Length >= 30) label4.Text = "";
			label4.Text += "  表单已保存";
		}
		//导入表单
		private void loadListView()
		{
			//导入学生信息
			listView1.Items.Clear();
			FileStream fs = new FileStream("StudentRecord.txt", FileMode.OpenOrCreate);
			StreamReader sr = new StreamReader(fs);
			string line = null;
			while ((line = sr.ReadLine()) != null)
			{
				string[] str = line.Split('*');
				listView1.Items.Add(new ListViewItem(new string[] { str[0], str[1], str[2], str[3], str[4], str[5], str[6] }));
			}
			fs.Close(); sr.Close();
			//导入账户信息
			listView2.Items.Clear();
			fs = new FileStream("UserRecord.txt", FileMode.OpenOrCreate);
			sr = new StreamReader(fs);
			while ((line = sr.ReadLine()) != null)
			{
				string[] str = line.Split('*');
				listView2.Items.Add(new ListViewItem(new string[] { str[0], str[1], str[2] }));
			}
			fs.Close(); sr.Close();
			//导入选课信息 仅当账户为学生
			if (3 == currentPermissionType)
			{
				fs = new FileStream("CourseRecord.txt", FileMode.OpenOrCreate);
				sr = new StreamReader(fs);
				while ((line = sr.ReadLine()) != null)
				{
					string[] str = line.Split('*');
					//如果找到与当前用户匹配的记录
					if (str[0] == currentUserName)
					{
						//向已选区导入课程
						for (int i = 1; (i + 1) < str.Length; i += 2)
						{
							listView4.Items.Add(new ListViewItem(new string[] { str[i], str[i + 1] }));
							TotalCourseSelectedCount++;
							TotalCredits += int.Parse(str[i + 1]);
						}
						break;
					}
				}
				//更新提示信息
				label7.Text = TotalCourseSelectedCount.ToString();
				label9.Text = TotalCredits.ToString();
				fs.Close(); sr.Close();
			}
			//导入待选课表
			listView3.Items.Clear();
			fs = new FileStream("CourseList.txt", FileMode.OpenOrCreate);
			sr = new StreamReader(fs);
			while ((line = sr.ReadLine()) != null)
			{
				string[] str = line.Split('*');
				listView3.Items.Add(new ListViewItem(new string[] { str[0], str[1], str[2], str[3], str[4] }));
			}
			fs.Close(); sr.Close();

			if (label4.Text.Length >= 30) label4.Text = "";
			label4.Text += "  记录已从文件导入";
		}
		//更新表单可见性
		private void updateListViewVisible()
		{
			//更新表单可见性
			if (oprMode == 1)
			{
				listView1.Visible = true;
				listView2.Visible = false;
				groupBox4.Visible = false;
			}
			else if (oprMode == 2)
			{
				listView1.Visible = false;
				listView2.Visible = true;
				groupBox4.Visible = false;
			}
			else if (oprMode == 3)
			{
				listView1.Visible = false;
				listView2.Visible = false;
				groupBox4.Visible = true;
				//对管理员和教师只开放选课浏览
				if (currentPermissionType == 1 || currentPermissionType == 2)
				{
					button1.Enabled = false; button2.Enabled = false;
				}
			}

			if (label4.Text.Length >= 30) label4.Text = "";
			label4.Text += "  新的表单已加载";
		}

		//更新操作模式
		private void updateOprMode(short _oprMode)
		{
			if (_oprMode == 1) { label4.Text = "已切换至学生信息管理模式"; label5.Text = "当前：信息管理模式"; }
			else if (_oprMode == 2) { label4.Text = "已切换至用户账户管理模式"; label5.Text = "当前：用户管理模式"; }
			else if (_oprMode == 3) { label4.Text = "已切换至选课模式"; label5.Text = "当前：选课模式"; }
			oprMode = _oprMode;
			updateListViewVisible();
		}
		//
		//载入信息
		//
		private void MainFormLoad()
		{
			//加载时间
			timer1.Start();
			//加载用户和权限
			label1.Text = "当前用户：" + currentUserName;
			switch (currentPermissionType)
			{
				case 0:
					label3.Text = "开发者";
					updateOprMode(1);
					break;
				case 1:
					label3.Text = "管理员";
					updateOprMode(1);
					break;
				case 2:
					label3.Text = "教师";
					updateOprMode(1);
					break;
				case 3:
					label3.Text = "学生";
					menuStrip1.Items[1].Enabled = false;    //禁用编辑菜单
					menuStrip1.Items[2].Enabled = false;    //禁用文件操作
					menuStrip1.Items[3].Enabled = false;    //禁用模式切换

					toolStrip1.Items[0].Enabled = false;    //禁用添加按钮
					toolStrip1.Items[1].Enabled = false;    //禁用修改按钮
					toolStrip1.Items[2].Enabled = false;    //禁用删除按钮
					updateOprMode(3);    //学生用户默认进入选课模式
					break;
				default: break;
			}
			//加载记录
			loadListView();
		}
		//显示时间
		private void timer1_Tick(object sender, EventArgs e)
		{
			label2.Text = DateTime.Now.ToString();
		}
		/////////////////////////////////////////////////////////////////////////////
		//系统菜单
		/////////////////////////////////////////////////////////////////////////////
		//返回登陆界面
		private void 注销ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form_LogIn formLogIn = new Form_LogIn();
			//为学生用户保存选课信息
			if (currentPermissionType == 3) saveListView();
			formLogIn.Show();
			this.Close();
		}
		//退出程序
		private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//为学生用户保存选课信息
			if (currentPermissionType == 3) saveListView();
			Application.Exit();
		}
		/////////////////////////////////////////////////////////////////////////////
		//编辑菜单
		/////////////////////////////////////////////////////////////////////////////
		//菜单栏添加选项
		private void 添加学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordAdd();
			else if (oprMode == 2) UserRecordAdd();
		}
		//菜单栏删除选项
		private void 删除学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordDelete();
			else if (oprMode == 2) UserRecordDelete();
		}
		//菜单栏编辑选项
		private void 修改学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordEdit();
			else if (oprMode == 2) UserRecordEdit();
		}
		//菜单栏查找选项
		private void 查找学生信息ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordSearch();
			else if (oprMode == 2) StudentRecordSearch();
			else if (oprMode == 3) StudentRecordSearch();
		}
		/////////////////////////////////////////////////////////////////////////////
		//文件菜单
		/////////////////////////////////////////////////////////////////////////////
		//文件菜单 保存按钮
		private void 保存ToolStripMenuItem_Click(object sender, EventArgs e) => saveListView();
		//文件菜单 导入按钮
		private void 导入ToolStripMenuItem_Click(object sender, EventArgs e) => loadListView();
		/////////////////////////////////////////////////////////////////////////////
		//模式菜单
		/////////////////////////////////////////////////////////////////////////////
		//切换至学生信息管理模式
		private void 学生信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (currentPermissionType == 3)
			{
				MessageBox.Show("权限不足");
				return;
			}
			if (oprMode != 1) { updateOprMode(1); updateListViewVisible(); }
		}
		//切换至用户账户管理模式
		private void 用户账户管理ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (currentPermissionType == 2 || currentPermissionType == 3)
			{
				MessageBox.Show("权限不足");
				return;
			}
			if (oprMode != 2) { updateOprMode(2); updateListViewVisible(); }
		}
		//切换至选课模式
		private void 选课信息管理ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (oprMode != 3) { updateOprMode(3); updateListViewVisible(); }
		}
		/////////////////////////////////////////////////////////////////////////////
		//工具栏
		/////////////////////////////////////////////////////////////////////////////
		//工具栏添加按钮
		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordAdd();
			else if (oprMode == 2) UserRecordAdd();
		}
		//工具栏删除按钮
		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordDelete();
			else if (oprMode == 2) UserRecordDelete();
		}
		//工具栏编辑按钮
		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordEdit();
			else if (oprMode == 2) UserRecordEdit();
		}
		//工具栏查找按钮
		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			if (oprMode == 1) StudentRecordSearch();
			else if (oprMode == 2) UserRecordSearch();
			else if (oprMode == 3) CourseRecordSearch();
		}
		/////////////////////////////////////////////////////////////////////////////
		//学生信息
		/////////////////////////////////////////////////////////////////////////////
		//
		//添加一条学生信息
		private void StudentRecordAdd()
		{
			Form_StudentEditer addForm = new Form_StudentEditer(this, listView1);
			addForm.Show();
		}
		//
		//删除选中学生信息
		private void StudentRecordDelete()
		{
			int deleteCount = 0;
			for (int i = 0; i <= (listView1.Items.Count - 1); ++i)
				if (listView1.Items[i].Selected == true)
				{
					++deleteCount;
					listView1.Items[i--].Remove(); //删除后更新i，以防跳过下一项
				}
			label4.Text = deleteCount.ToString() + "条记录已从库中删除";
		}
		//
		//修改选中学生信息
		private void StudentRecordEdit()
		{
			if (listView1.SelectedItems.Count == 1)
			{
				int _index = 0;
				for (int i = 0; i <= (listView1.Items.Count - 1); ++i)
					if (listView1.Items[i].Selected == true)
					{
						_index = i;
						break;
					}

				Form_StudentEditer editForm = new Form_StudentEditer(this, listView1, _index);
				editForm.Show();
			}
			else
			{
				label4.Text = "请选择一条记录进行编辑！";
				return;
			}
		}
		//
		//查找学生信息
		private void StudentRecordSearch()
		{
			Form_Search formSearch = new Form_Search(this, listView1);
			formSearch.Show();
		}
		///////////////////////////////////////////////////////////////////////////
		//账户管理
		///////////////////////////////////////////////////////////////////////////
		//添加新账户
		private void UserRecordAdd()
		{
			Form_UserEditer addForm = new Form_UserEditer(this, listView2);
			addForm.Show();
		}
		//修改账户信息
		private void UserRecordEdit()
		{
			if (listView2.SelectedItems.Count == 1)
			{
				int _index = 0;
				for (int i = 0; i <= (listView2.Items.Count - 1); ++i)
					if (listView2.Items[i].Selected == true)
					{
						_index = i;
						break;
					}

				Form_UserEditer editForm = new Form_UserEditer(this, listView2, _index);
				editForm.Show();
			}
			else
			{
				label4.Text = "请选择一条记录进行编辑！";
				return;
			}
		}
		//删除选中账户
		private void UserRecordDelete()
		{
			int deleteCount = 0;
			for (int i = 0; i <= (listView2.Items.Count - 1); ++i)
				if (listView2.Items[i].Selected == true)
				{
					++deleteCount;
					listView2.Items[i--].Remove(); //删除后更新i，以防跳过下一项
				}
			label4.Text = deleteCount.ToString() + "条记录已从库中删除";
		}
		//查找账户
		private void UserRecordSearch()
		{
			Form_Search formSearch = new Form_Search(this, listView2);
			formSearch.Show();
		}
		/////////////////////////////////////////////////////////////////////////////
		//学生选课
		/////////////////////////////////////////////////////////////////////////////
		//课程查询
		private void CourseRecordSearch()
		{
			Form_Search formSearch = new Form_Search(this, listView3);
			formSearch.Show();
		}
		//课程查重
		private Boolean CourseChecked(ListViewItem lvi)
		{
			bool flag = false;
			for (int i = 0; i <= (listView4.Items.Count - 1); ++i)
			{
				if (listView4.Items[i].SubItems[0].Text == lvi.SubItems[0].Text)
				{
					flag = true;
					break;
				}
			}
			return flag;
		}
		//添加课程按钮
		private void button1_Click(object sender, EventArgs e)
		{
			int selectCount = 0;
			ListViewItem lvi;
			for (int i = 0; i <= (listView3.Items.Count - 1); ++i)
				if (listView3.Items[i].Selected == true)
				{
					if (CourseChecked(listView3.Items[i]) == true) continue;

					++selectCount;
					lvi = new ListViewItem(); lvi.SubItems.Clear();
					lvi.SubItems[0].Text = listView3.Items[i].SubItems[0].Text;
					lvi.SubItems.Add(listView3.Items[i].SubItems[3].Text);
					listView4.Items.Add(lvi);

					TotalCourseSelectedCount++;
					TotalCredits += short.Parse(listView3.Items[i].SubItems[3].Text);
				}

			label4.Text = selectCount.ToString() + "门课程已添加";
			label7.Text = TotalCourseSelectedCount.ToString();
			label9.Text = TotalCredits.ToString();
		}
		//移除课程按钮
		private void button2_Click(object sender, EventArgs e)
		{
			int deleteCount = 0;
			for (int i = 0; i <= (listView4.Items.Count - 1); ++i)
				if (listView4.Items[i].Selected == true)
				{
					++deleteCount;
					TotalCourseSelectedCount--;
					TotalCredits -= short.Parse(listView4.Items[i].SubItems[1].Text);
					listView4.Items[i--].Remove(); //删除后更新i，以防跳过下一项
				}
			label4.Text = deleteCount.ToString() + "门课程已删除";
			label7.Text = TotalCourseSelectedCount.ToString();
			label9.Text = TotalCredits.ToString();
		}
	}
}
