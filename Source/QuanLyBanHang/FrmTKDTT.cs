using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using app = Microsoft.Office.Interop.Excel.Application;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyBanHang
{
    public partial class FrmTKDTT : Form
    {
        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();
        SqlConnection con = Connection.connect;
        string CurrentMonth = DateTime.Now.ToString("MM");

        public FrmTKDTT()
        {
            InitializeComponent();
        }

        public void SetMyCustomFormat()
        {
            // Set the Format type and the CustomFormat string.
            dtNam.Format = DateTimePickerFormat.Custom;
            dtNam.CustomFormat = "yyyy";
        }


        private void FrmTKDTT_Load(object sender, EventArgs e)
        {
            SetMyCustomFormat();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtThang.Text.Trim().Length.Equals(0) && dtNam.Text.Trim().Length.Equals(0))
                {
                    MessageBox.Show("Please choose a date to analyze.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dataThongKe.DataSource = null;
                }
                else if (!Model.checkIsDigit(txtThang.Text.Trim()))
                {
                    if (txtThang.Text.Trim().Length.Equals(0))
                    {
                        dataThongKe.DataSource = from a in db.HoaDons
                                                 where a.NgayBan.Value.Year.Equals(dtNam.Text.Trim())
                                                 select new
                                                 {
                                                     a.MaHD,
                                                     a.NgayBan,
                                                     a.MaKH,
                                                     a.MaNV,
                                                     a.TongTien
                                                 };
                        var tongTien = db.HoaDons.Where(n => n.NgayBan.Value.Year.Equals(dtNam.Text.Trim())).Sum(n => n.TongTien);
                        if (tongTien == null)
                        {
                            lbTongDoanhThu.Text = 0 + " VNĐ";
                        }
                        else
                        {
                            lbTongDoanhThu.Text = String.Format("{0:0,0}", tongTien) + " VNĐ";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Month is invalid.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        dataThongKe.DataSource = null;
                        txtThang.Focus();
                    }
                }
                else if (int.Parse(txtThang.Text.Trim()) > 12 || int.Parse(txtThang.Text.Trim()) < 1)
                {
                    MessageBox.Show("Month is invalid.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    dataThongKe.DataSource = null;
                    txtThang.Focus();
                }
                else
                {
                    dataThongKe.DataSource = from a in db.HoaDons
                                             where a.NgayBan.Value.Month.Equals(txtThang.Text.Trim()) && a.NgayBan.Value.Year.Equals(dtNam.Text.Trim())
                                             select new
                                             {
                                                 a.MaHD,
                                                 a.NgayBan,
                                                 a.MaKH,
                                                 a.MaNV,
                                                 a.TongTien
                                             };
                    var tongTien = db.HoaDons.Where(n => n.NgayBan.Value.Month.Equals(txtThang.Text.Trim()) && n.NgayBan.Value.Year.Equals(dtNam.Text.Trim())).Sum(n => n.TongTien);
                    if (tongTien == null)
                    {
                        lbTongDoanhThu.Text = 0 + " VNĐ";
                    }
                    else
                    {
                        lbTongDoanhThu.Text = String.Format("{0:0,0}", tongTien) + " VNĐ";
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void xuatfileExcel(DataGridView g, string path, string tenfile)
        {
            app obj = new app();
            obj.Application.Workbooks.Add(Type.Missing);
            obj.Columns.ColumnWidth = 30;

            for (int i = 1; i < g.ColumnCount + 1; i++)
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)
            {
                for (int j = 0; j < g.Columns.Count; j++)
                {
                    if (g.Rows[i].Cells[j].Value != null)
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            obj.ActiveWorkbook.SaveCopyAs(path + tenfile + ".xlsx");
            obj.ActiveWorkbook.Saved = true;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dataThongKe.DataSource == null)
            {
                MessageBox.Show("There is no data for exporting.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else {           
                xuatfileExcel(dataThongKe, @"C:\Users\Admin\Pictures\", "Revenue");
                MessageBox.Show("File exported successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
