using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.IO;

namespace ReportTemplateTest
{
	public partial class Form1 : XtraForm 
	{
		public Form1()
		{
			InitializeComponent();
		}

		private XtraReport CreateReport()
		{
			DataTable dataSource = new DataTable("DataSourceName");
			dataSource.Columns.Add(new DataColumn("ColumnNameTest", typeof(int)));

			var report = new XtraReport();

			// Создаем элементы шаблона отчета
			var headerBand = new ReportHeaderBand();
			report.Bands.Add(headerBand);

			var detailBand = new DetailBand
			{
				Height = 23
			};
			report.Bands.Add(detailBand);

			var label = new XRLabel
			{
				Text = "Заголовок отчета"
			};
			headerBand.Controls.Add(label);

			var cell = new XRTableCell
			{
				Borders = DevExpress.XtraPrinting.BorderSide.All
			};
			cell.DataBindings.Add("Text", null, "ColumnNameTest", "{0}");
			detailBand.Controls.Add(cell);

			// Привязка шаблона к отчетной форме
			report.DataSource = dataSource;
			report.DataMember = "DataSourceName";

			// Заполняем источник данных
			for (int i = 0; i < 10; i++)
			{
				DataRow row = dataSource.NewRow();
				row["ColumnNameTest"] = i * 10;
				dataSource.Rows.Add(row);
			}

			// Создание отчета (связываем шаблон с данными)
			report.CreateDocument();
			return report;
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			var report = CreateReport();
			
			// Отображаем созданный отчет
			report.ShowPreview();
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			var report = CreateReport();
			byte[] buffer = null;

			using (var stream = new MemoryStream())
			{
				report.SaveLayoutToXml(stream);
				buffer = stream.ToArray();
			}

			// Создаем новый отчет из сохраненного массива байт
			var rpt = new XtraReport();
			using (var stream = new MemoryStream(buffer))
			{
				rpt.LoadLayoutFromXml(stream);
			}

			// Отображаем новый отчет
			rpt.ShowPreview();
		}

		private void simpleButton3_Click(object sender, EventArgs e)
		{
			var report = CreateReport();
			byte[] buffer = null;

			using (var stream = new MemoryStream())
			{
				report.PrintingSystem.SaveDocument(stream);
				buffer = stream.ToArray();
			}

			// Создаем новый отчет из сохраненного массива байт
			var rpt = new XtraReport();
			using (var stream = new MemoryStream(buffer))
			{
				rpt.PrintingSystem.LoadDocument(stream);
			}

			// Отображаем новый отчет
			rpt.ShowPreview();
		}
	}		    
}