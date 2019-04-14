using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uçak_Bileti_Satış_Otomasyonu
{
    public partial class OdemeBilgileri : Form
    {
        public OdemeBilgileri()
        {
            InitializeComponent();
        }

        #region Değişkenler/Fields (Alanlar)

        //Panel ile Form'u Sürüklemek için alttaki 2 field'ı tanımlamak zorundayız.
        private const int HT_CAPTION = 0x2;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        //---------------------------------------------------------------------------

        Anasayfa anasayfa = (Anasayfa)Application.OpenForms["Anasayfa"];
        UcusListeleri uL = (UcusListeleri)Application.OpenForms["UcusListeleri"];
        UcusSec uS = (UcusSec)Application.OpenForms["UcusSec"];
        UcusOzet uO = new UcusOzet();


        public string tcKimlikNo, ad, soyad, cinsiyet, dogumTarihi, cepTelefonu, ePosta, gidisNoktasi, varisNoktasi, gidisTarihi, donusTarihi;
        public string biletSinifi, tekCiftYon, bagajKutlesi, firma, biletFiyati,biletNo;

        public string gidisFirma, sinifi;

        public int yetiskinSayisi, cocukSayisi, yolcuSayisi;

        public string tabloIsmi;

        bool d_kartNo, d_Ay, d_Yil, d_güvenlikKodu, d_adSoyad;

        SQLiteConnection sqlConn = null;
        SQLiteCommand sqlComm = null;

        #endregion

        #region Panel ile Form'u Sürükleme

        private void pnlUst_MouseDown(object sender, MouseEventArgs e)
        {
            // Panel'e ya da Form'a gelen Mouse hareketlerini yakalama (Capture = false).
            (sender as Control).Capture = false;

            // Sanki başlık çubuğu (Caption Bar) üzerinde sol Mou butonu tıklaması
            // başlamış gibi yap: 1) önce sahte mesajı oluştur.

            Message msg = Message.Create(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);

            // 2) Sonra sahte mesajı uygulamanın WndProc() metoduna gönder.
            base.WndProc(ref msg);
        }

        #endregion


        private void OdemeBilgileri_Load(object sender, EventArgs e)
        {
            #region Design

            this.BackColor = Color.FromArgb(35, 47, 63); //Siyah renk.

            picKirmiziCizgi1.BackColor = Color.FromArgb(231, 72, 86); //Kırmızı renk.

            lblToplamTutarAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblToplamTutarFiyat.BackColor = Color.FromArgb(35, 47, 63);


            txtBankaKartNo.BackColor = Color.FromArgb(35, 47, 63);

            txtAy.BackColor = Color.FromArgb(35, 47, 63);
            txtYil.BackColor = Color.FromArgb(35, 47, 63);
            txtGuvenlikKodu.BackColor = Color.FromArgb(35, 47, 63);
            txtKartAdSoyad.BackColor = Color.FromArgb(35, 47, 63);

            lblGuvenlikKoduAciklama.BackColor = Color.FromArgb(35, 47, 63);
            lblGuvenlikKoduAciklama.Text = "(Kartınızın arka yüzündeki\n          son 3 rakamdır.)";

            #endregion

            uL.Visible = false;

        }


        #region textBox'ların Klavye Tuş Bastırmama Durumlarını Kontrol Etme

        private void txtBankaKartNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            //16 karakterli

            if ((e.KeyChar >= 48) && (e.KeyChar <= 57))
                e.Handled = false; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdır.
            else if (e.KeyChar == 8)
                e.Handled = false; //Backspace (silme) tuşunu yazdır.
            else if (e.KeyChar == 32)
                e.Handled = false; //Boşluk (space) tuşunu yazdır.
            else
                e.Handled = true;
        }

        private void txtAy_KeyPress(object sender, KeyPressEventArgs e)
        {
            //2 karakterli

            if ((e.KeyChar >= 48) && (e.KeyChar <= 57))
                e.Handled = false; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdır.
            else if (e.KeyChar == 8)
                e.Handled = false; //Backspace (silme) tuşunu yazdır.
            else if (e.KeyChar == 32)
                e.Handled = false; //Boşluk (space) tuşunu yazdır.
            else
                e.Handled = true;
        }

        private void txtYil_KeyPress(object sender, KeyPressEventArgs e)
        {
            //2 karakterli

            if ((e.KeyChar >= 48) && (e.KeyChar <= 57))
                e.Handled = false; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdır.
            else if (e.KeyChar == 8)
                e.Handled = false; //Backspace (silme) tuşunu yazdır.
            else if (e.KeyChar == 32)
                e.Handled = false; //Boşluk (space) tuşunu yazdır.
            else
                e.Handled = true;
        }

        private void txtGuvenlikKodu_KeyPress(object sender, KeyPressEventArgs e)
        {
            //3 karakterli

            if ((e.KeyChar >= 48) && (e.KeyChar <= 57))
                e.Handled = false; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdır.
            else if (e.KeyChar == 8)
                e.Handled = false; //Backspace (silme) tuşunu yazdır.
            else if (e.KeyChar == 32)
                e.Handled = false; //Boşluk (space) tuşunu yazdır.
            else
                e.Handled = true;
        }

        private void txtKartAdSoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 33) && (e.KeyChar <= 64))
                e.Handled = true; //Klavyeden eğer sayı tuşlarına basılırsa bunları yazdırma.
            else if ((e.KeyChar >= 91) && (e.KeyChar <= 96))
                e.Handled = true;
            else if ((e.KeyChar >= 123) && (e.KeyChar <= 127))
                e.Handled = true;
            else
                e.Handled = false; //Yukarıdaki ASCII kodlarına karşılık gelen tuşların haricinde olan bütün tuşların basılmasını izin ver.
        }



        #endregion



        #region Ödemeyi Tamamen Bitirme

        void veriTabaninaVerileriEklemek()
        {
            sqlConn = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\UcakBiletSatis.db");
            sqlConn.Open();

            sqlComm = new SQLiteCommand(sqlConn);

            sqlComm.CommandText = "INSERT INTO '" + tabloIsmi + "'(TCKimlikNo,Ad,Soyad,Cinsiyet,DogumTarihi,CepTelefonu,ePosta,GidisNoktasi,VarisNoktasi,GidisTarihi,DonusTarihi,BiletSinifi,TekCiftYon,YetiskinSayisi,CocukSayisi,YolcuSayisi,BagajKutlesi,Firma,BiletFiyati,BiletNo) VALUES(@TCKimlikNo,@Ad,@Soyad,@Cinsiyet,@DogumTarihi,@CepTelefonu,@ePosta,@GidisNoktasi,@VarisNoktasi,@GidisTarihi,@DonusTarihi,@BiletSinifi,@TekCiftYon,@YetiskinSayisi,@CocukSayisi,@YolcuSayisi,@BagajKutlesi,@Firma,@BiletFiyati,@BiletNo)";

            sqlComm.Parameters.AddWithValue("@TCKimlikNo", tcKimlikNo);
            sqlComm.Parameters.AddWithValue("@Ad", ad);
            sqlComm.Parameters.AddWithValue("@Soyad", soyad);
            sqlComm.Parameters.AddWithValue("@Cinsiyet", cinsiyet);
            sqlComm.Parameters.AddWithValue("@DogumTarihi", dogumTarihi);
            sqlComm.Parameters.AddWithValue("@CepTelefonu", cepTelefonu);
            sqlComm.Parameters.AddWithValue("@ePosta", ePosta);
            sqlComm.Parameters.AddWithValue("@GidisNoktasi", gidisNoktasi);
            sqlComm.Parameters.AddWithValue("@VarisNoktasi", varisNoktasi);
            sqlComm.Parameters.AddWithValue("@GidisTarihi", gidisTarihi);
            sqlComm.Parameters.AddWithValue("@DonusTarihi", donusTarihi);
            sqlComm.Parameters.AddWithValue("@BiletSinifi", biletSinifi);
            sqlComm.Parameters.AddWithValue("@TekCiftYon", tekCiftYon);
            sqlComm.Parameters.AddWithValue("@YetiskinSayisi", yetiskinSayisi);
            sqlComm.Parameters.AddWithValue("@CocukSayisi", cocukSayisi);
            sqlComm.Parameters.AddWithValue("@YolcuSayisi", yolcuSayisi);
            sqlComm.Parameters.AddWithValue("@BagajKutlesi", bagajKutlesi);
            sqlComm.Parameters.AddWithValue("@Firma", firma);
            sqlComm.Parameters.AddWithValue("@BiletFiyati", biletFiyati);
            sqlComm.Parameters.AddWithValue("@BiletNo", biletNo);


            sqlComm.ExecuteNonQuery();

        }

        private void ambiance_Button_OdemeyeBitir_Click(object sender, EventArgs e)
        {
            try
            {
                #region Banka Kart No

                if (txtBankaKartNo.Text.Length < 16)
                {
                    d_kartNo = false;

                    errorProvTxBankaKartNo.SetError(txtBankaKartNo, "Banka Kart numarası 16 haneli olmalıdır.");
                }
                else
                    d_kartNo = true;

                #endregion

                #region Ay

                if (txtAy.Text.Length < 2)
                {
                    d_Ay = false;

                    errorProvTxtAy.SetError(txtAy, "Son kullanma tarihinin ay kısmı 2 haneli olmalıdır.");
                }
                else
                    d_Ay = true;

                #endregion

                #region Yıl

                if (txtYil.Text.Length < 2)
                {
                    d_Yil = false;

                    errorProvTxtYil.SetError(txtYil, "Son kullanma tarihinin yıl kısmı 2 haneli olmalıdır.");
                }
                else
                    d_Yil = true;

                #endregion

                #region Güvenlik Kodu

                if (txtGuvenlikKodu.Text.Length < 3)
                {
                    d_güvenlikKodu = false;

                    errorProvGüvenlikKodu.SetError(txtGuvenlikKodu, "Son kullanma tarihinin yıl kısmı 2 haneli olmalıdır.");
                }
                else
                    d_güvenlikKodu = true;

                #endregion

                #region Banka Kartı Üzerindeki Ad ve Soyad

                if (txtKartAdSoyad.Text == "")
                {
                    d_adSoyad = false;

                    errorProvTxtAdSoyad.SetError(txtKartAdSoyad, "Banka kartının üzerindeki ad ve soyad boş geçilemez.");
                }
                else
                    d_adSoyad = true;

                #endregion

                if ((d_kartNo == true) && (d_Ay == true) && (d_Yil == true) && (d_güvenlikKodu == true) && (d_adSoyad == true))
                {
                    veriTabaninaVerileriEklemek();

                    uO.lblGidisFirma.Text = firma;
                    uO.lblSinifi.Text = biletSinifi;
                    uO.lblKalkisKonum.Text = anasayfa.txtGidisNoktasi.Text;
                    uO.lblVarisKonum.Text = anasayfa.txtInisNoktasi.Text; ;
                    uO.lblBagajHakki.Text = bagajKutlesi;
                    uO.lblOlusturmaTarihi.Text = DateTime.Now.ToString();

                    uO.lblGenelToplam.Text = lblToplamTutarFiyat.Text;

                    uO.lblYolcuSayisi.Text = yolcuSayisi.ToString();
                    uO.lblAdiSoyadi.Text = ad + " " + soyad;
                    uO.lblDogumTarihi.Text = dogumTarihi;
                    uO.lblBiletNo.Text = biletNo;

                    uO.lblBitisAciklama.Text = "Ödemeniz başarıyla gerçekleşti, uçuş bilgileriniz ve ve e-biletiniz vermiş olduğunuz  \'" + ePosta + "\'  adresinize iletildi.\nBiletin ulaştığından emin olmak için lütfen e-posta kutunuzu kontrol ediniz.\n\nBiletinizle ilgili sorularınız olduğunda ya da değişiklik yapmak istediğiniz çağrı ekibimizi 7 gün 24 saat arayabilirsiniz.\n\nHayırlı uçuşlar dileriz.\n\n";


                    if (uO.lblGidisFirma.Text == "Turkish Airlines")
                        uO.picGidisPhoto.Image = uO.ımageList_Havayollari.Images[4];
                    else if (uO.lblGidisFirma.Text == "Anadolu Jet")
                        uO.picGidisPhoto.Image = uO.ımageList_Havayollari.Images[0];
                    else if (uO.lblGidisFirma.Text == "Atlas Global")
                        uO.picGidisPhoto.Image = uO.ımageList_Havayollari.Images[1];
                    else if (uO.lblGidisFirma.Text == "Onur Air")
                        uO.picGidisPhoto.Image = uO.ımageList_Havayollari.Images[2];
                    else if (uO.lblGidisFirma.Text == "Pegasus")
                        uO.picGidisPhoto.Image = uO.ımageList_Havayollari.Images[3];

                    
                    uO.Show();

                    this.Visible = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion


        private void btnKapat_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnAsagiIndir_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
