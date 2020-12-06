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
	public partial class Form_Search : Form
	{
		//父窗体
		Form_Main preForm = null;
		//当前操作表单
		ListView preListView = null;
		//当前表单索引总数
		private int preItemsCount = 0;
		//当前查找列
		private int preColumn = -1;
		//查找结果标记
		private Boolean flag = false;

		public Form_Search(Form_Main _preForm, ListView _lv)
		{
			InitializeComponent();
			preForm = _preForm; preListView = _lv;
			preItemsCount = _lv.Items.Count;

			//重新加载列表内容
			listBox1.Items.Clear();
			for(int i = 0; i < preListView.Columns.Count; ++ i)
			{
				listBox1.Items.Add(preListView.Columns[i].Text);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if(preColumn == -1)
			{
				MessageBox.Show("请完善信息!");
				return;
			}

			preListView.SelectedItems.Clear();
			for (int preIndex = 0; preIndex < preItemsCount; ++ preIndex)
			{
				if (preListView.Items[preIndex].SubItems[preColumn].Text == textBox1.Text)
				{
					preListView.SelectedIndices.Add(preIndex);
					flag = true; 
				}
			}

			if(true == flag)
			{
				preForm.label4.Text = "查询到" + preListView.SelectedIndices.Count.ToString() +"条结果";
				this.Close();
			}
			else
			{
				preForm.label4.Text = "未找到相应结果";
				this.Close();
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			flag = false;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			flag = false;
			preColumn = listBox1.SelectedIndex;
		}
	}
}
