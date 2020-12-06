using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentInformationManageSystem
{
	//用于添加和修改操作时显示信息及操作
	public partial class Form_StudentEditer : Form
	{
		ListView preListView = null;
		Form_Main contentForm = null;
		private readonly int oprType = 0;
		private readonly int index = 0;
		//
		//初始化
		//添加操作 初始化
		public Form_StudentEditer(Form_Main _currentForm2, ListView _lv)
		{
			InitializeComponent();
			contentForm = _currentForm2; preListView = _lv; oprType = 1;
		}
		//编辑操作 初始化
		public Form_StudentEditer(Form_Main _currentForm2, ListView _lv, int _index)
		{
			InitializeComponent();
			contentForm = _currentForm2; preListView = _lv; index = _index; oprType = 2;
			//向操作窗体内填入信息
			ListViewItem lvi = preListView.Items[index];
			textBox1.Text = lvi.SubItems[0].Text;
			textBox2.Text = lvi.SubItems[1].Text;
			textBox3.Text = lvi.SubItems[2].Text;
			textBox4.Text = lvi.SubItems[3].Text;
			textBox5.Text = lvi.SubItems[5].Text;
			textBox6.Text = lvi.SubItems[6].Text;
			listBox1.SelectedIndex = listBox1.FindString(lvi.SubItems[4].Text);
		}
		//
		//信息编辑
		//将填入信息整合为ListViewItem
		private ListViewItem getContent()
		{
			ListViewItem lvi = new ListViewItem();
			lvi.SubItems.Clear();
			lvi.SubItems[0].Text = textBox1.Text;
			lvi.SubItems.Add(textBox2.Text);
			lvi.SubItems.Add(textBox3.Text);
			lvi.SubItems.Add(textBox4.Text);
			//特判是否已选择性别
			if (listBox1.SelectedIndex != -1)
				lvi.SubItems.Add(listBox1.SelectedItem.ToString());
			else
				lvi.SubItems.Add("");
			lvi.SubItems.Add(textBox5.Text);
			lvi.SubItems.Add(textBox6.Text);
			return lvi;
		}
		//确认 添加信息 
		private void AddRecord()
		{   
			//检查信息是否有效
			if (textBox1.Text.Equals(String.Empty) && textBox2.Text.Equals(String.Empty))
			{
				MessageBox.Show("请至少输入姓名或学号！");
				return;
			}
			//向listview中添加新项
			ListViewItem lvi = getContent();
			preListView.Items.Add(lvi);

			String messsageStr = "信息 ";
			if (!textBox1.Text.Equals(String.Empty)) messsageStr += textBox1.Text;
			if (!textBox2.Text.Equals(String.Empty)) messsageStr += " " + textBox2.Text;
			//MessageBox.Show(messsageStr + " 添加成功");
			contentForm.label4.Text = messsageStr + " 添加成功";
		}
		//确认 修改信息
		private void EditRecord()
		{
			//检查信息是否有效
			if (textBox1.Text.Equals(String.Empty) && textBox2.Text.Equals(String.Empty))
			{
				MessageBox.Show("请至少输入姓名或学号！");
			}
			//用新项替换原记录
			ListViewItem lvi = getContent();
			preListView.Items[index] = lvi;

			String messsageStr = "信息 ";
			if (!textBox1.Text.Equals(String.Empty)) messsageStr += textBox1.Text;
			if (!textBox2.Text.Equals(String.Empty)) messsageStr += " " + textBox2.Text;
			//MessageBox.Show(messsageStr + " 修改成功");
			contentForm.label4.Text = messsageStr + " 修改成功";
		}
		//
		//button响应函数
		//确认
		private void button1_Click(object sender, EventArgs e)
		{
			int temp = preListView.Items.Count;
			switch (oprType)
			{
				case 1: AddRecord(); break;
				case 2: EditRecord(); break;
				default: break;
			}
			//关闭当前窗口
			switch (oprType)
			{
				case 1: if (preListView.Items.Count - temp == 1) this.Close(); break;
				case 2: EditRecord(); break;
				default: break;
			}
		}
		//取消
		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
