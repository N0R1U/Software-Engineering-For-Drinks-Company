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
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyBanHang
{
    public partial class FrmThongKe : Form
    {

        DBQuanLyBanHangDataContext db = new DBQuanLyBanHangDataContext();

        SqlConnection con = Connection.connect;

        public FrmThongKe()
        {
            InitializeComponent();
        }

        private void ChartMoney() {
            DataSet ds = new DataSet();
            con.Open();
            
            SqlDataAdapter adapt = new SqlDataAdapter("select top 5 TenSP,DaBan from SanPham order by DaBan desc", con);
            adapt.Fill(ds);
            chart1.DataSource = ds;
            chart1.Series["Sold"].XValueMember = "TenSP";
            chart1.Series["Sold"].YValueMembers = "DaBan";
            Title title = new Title();
            title.Font = new Font("Times new Roman", 14, FontStyle.Bold);
            title.Text = "Top 5 best seller.";
            chart1.Titles.Add(title);
            chart1.Series["Sold"].IsValueShownAsLabel = true;
            con.Close();
        }

        private void FrmThongKe_Load(object sender, EventArgs e)
        {
            ChartMoney();
            TongDonHang();
            TongDoanhThu();
            TongDonHangHomNay();
            timer1.Start();
        }

        private void TongDonHang() {
            lbelTongDH.Text = db.HoaDons.Count().ToString();
        }

        private void TongDonHangHomNay() {
            var totay = DateTime.Today;
            lbelTongDHHomNay.Text = db.HoaDons.Where(n => n.NgayBan.Value.Date == totay).Count().ToString() + " orders";
        }

        private void TongDoanhThu() {
            lbelTongDoanhThu.Text = String.Format("{0:0,0}", db.HoaDons.Sum(n => n.TongTien)) + " VNĐ";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lbelDate.Text = dt.ToString("dd/MM/yyyy");
            lbelTime.Text = dt.ToString("HH:mm:ss");
        }

        private void iconPictureBox9_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmUser frmuser = new FrmUser();
            frmuser.ShowDialog();
        }

        private void iconPictureBox8_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmUser fuser = new FrmUser();
            fuser.ShowDialog();
        }

        private void iconPictureBox7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmQLNhanVien frmnv = new FrmQLNhanVien();
            frmnv.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmTaiKhoan frmtk = new FrmTaiKhoan();
            frmtk.ShowDialog();
        }

        private void btnNhapKho_Click(object sender, EventArgs e)
        {
            FrmNhapKho frmnhapkho = new FrmNhapKho();
            frmnhapkho.ShowDialog();
        }

        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            FrmDonHang frmdonhang = new FrmDonHang();
            frmdonhang.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmTKDTT tk = new FrmTKDTT();
            tk.ShowDialog();
        }
    }
}
