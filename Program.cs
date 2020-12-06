using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace StudentInformationManageSystem
{
	static class Program
	{
		
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			DataSet ds = new DataSet();
			DataTable dt1 = new DataTable("users");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			new Form_LogIn().Show();
			Application.Run();

			Application.Exit();
			
		}
	}
}
