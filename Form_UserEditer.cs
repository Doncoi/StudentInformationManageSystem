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
	public partial class Form_UserEditer : Form
	{
		ListView preListView = null;
		Form_Main contentForm = null;
		private readonly int oprType = 0;
		private readonly int index = 0;

		public Form_UserEditer()
		{
			InitializeComponent();
		}
		//添加操作
		public Form_UserEditer(Form_Main _currentForm2, ListView _lv)
		{
			InitializeComponent();
			contentForm = _currentForm2; preListView = _lv; oprType = 1;
		}
		//修改操作
		public Form_UserEditer(Form_Main _currentForm2, ListView _lv, int _index)
		{
			InitializeComponent();
			contentForm = _currentForm2; preListView = _lv; index = _index; oprType = 2;
			//向操作窗体内填入信息
			ListViewItem lvi = preListView.Items[_index];
			textBox1.Text = lvi.SubItems[0].Text;
			textBox2.Text = lvi.SubItems[1].Text;
			listBox1.SelectedIndex = listBox1.FindString(lvi.SubItems[2].Text);
		}
		//整合填入信息为ListViewItem
		private ListViewItem getContent()
		{
			ListViewItem lvi = new ListViewItem();
			lvi.SubItems.Clear();
			lvi.SubItems[0].Text = textBox1.Text;
			lvi.SubItems.Add(textBox2.Text);
			//特判是否已选择性别
			lvi.SubItems.Add(listBox1.SelectedItem.ToString());
			return lvi;
		}
		//确认 添加信息
		private void AddRecord()
		{
			//检查信息是否有效
			if (textBox1.Text.Equals(String.Empty) || textBox2.Text.Equals(String.Empty))
			{ MessageBox.Show("请填充用户名和密码"); return;}
			if (listBox1.SelectedIndex == -1) {MessageBox.Show("请选择账户类型");return;}
			//向listview中添加新项
			ListViewItem lvi = getContent();
			preListView.Items.Add(lvi);
			//提示信息添加成功
			String messsageStr = listBox1.SelectedItem.ToString() + " " + textBox1.Text;
			contentForm.label4.Text = messsageStr + " 添加成功";
		}
		//确认 修改信息
		private void EditRecord()
		{
			//检查信息是否有效
			if (textBox1.Text.Equals(String.Empty) || textBox2.Text.Equals(String.Empty))
			{ MessageBox.Show("请填充用户名和密码"); return; }
			if (listBox1.SelectedIndex == -1) { MessageBox.Show("请选择账户类型"); return; }
			//用新项替换原记录
			ListViewItem lvi = getContent();
			preListView.Items[index] = lvi;
			//提示信息修改成功
			String messsageStr = listBox1.SelectedItem.ToString() + " " + textBox1.Text;
			contentForm.label4.Text = messsageStr + " 修改成功";
		}
		//确认按钮
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
		//取消按钮
		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
