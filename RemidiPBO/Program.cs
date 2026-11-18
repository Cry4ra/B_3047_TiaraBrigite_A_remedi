using System;
using System.Collections.Generic;

namespace RemidiPBO
{
    class Sekolah
    {
        public string idSekolah { get; set; }
        public string namaSekolah { get; set; }
        public int jumlahPaketDiterima { get; set; }

        public Sekolah(string id, string nama, int jumlah)
        {
            idSekolah = id;
            namaSekolah = nama;
            jumlahPaketDiterima = jumlah;
        }

        public void TampilInfo()
        {
            Console.WriteLine("ID Sekolah: " + idSekolah);
            Console.WriteLine("Nama Sekolah: " + namaSekolah);
            Console.WriteLine("Jumlah Paket Diterima: " + jumlahPaketDiterima);
            Console.WriteLine("===================================");
        }
    }

    public abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;

        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }

        public int StokDapur
        {
            get { return stokDapur; }
            set
            {
                if (value >= 0)
                    stokDapur = value;
                else
                    Console.WriteLine("Wah, stok nggak boleh minus!");
            }
        }

        public PaketMBG(string id, string nama, int stok)
        {
            idPaket = id;
            namaPaket = nama;
            StokDapur = stok;
        }

        public void KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stokDapur)
            {
                stokDapur -= jumlah;
            }
        }

        public abstract double HitungBiaya(int jumlah);
    }

    class PaketReguler : PaketMBG
    {
        public PaketReguler(string id, string nama, int stok) : base(id, nama, stok)
        {
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 15000;
        }
    }

    class PaketKhusus : PaketMBG
    {
        public string jenisDiet { get; set; }

        public PaketKhusus(string id, string nama, int stok, string diet) : base(id, nama, stok)
        {
            jenisDiet = diet;
        }

        public override double HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    class DistribusiMBG : DapatDidistribusikan
    {
        public Sekolah sekolahTujuan { get; set; }
        public PaketMBG paketMakanan { get; set; }
        public int jumlahKirim { get; set; }

        public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
        {
            sekolahTujuan = sekolah;
            paketMakanan = paket;
            jumlahKirim = jumlah;
        }

        public void KirimPaket()
        {
            Console.WriteLine("\nLagi proses kirim ke " + sekolahTujuan.namaSekolah + " nih");

            if (jumlahKirim > 0 && jumlahKirim <= paketMakanan.StokDapur)
            {
                paketMakanan.KurangiStok(jumlahKirim);
                sekolahTujuan.jumlahPaketDiterima += jumlahKirim;

                Console.WriteLine("Mantap, pengiriman sukses! Sebanyak " + jumlahKirim + " paket udah nyampe di " + sekolahTujuan.namaSekolah);
                Console.WriteLine("Sisa stok di dapur sisa: " + paketMakanan.StokDapur);
            }
            else
            {
                Console.WriteLine("Duh, pengirimannya batal. Jumlah yang dikirim gak beres atau stok dapurnya kurang");
            }
        }
    }

    class PengelolaMBG
    {
        private List<Sekolah> listSekolah = new List<Sekolah>();
        private List<PaketMBG> listPaket = new List<PaketMBG>();

        public void TambahSekolah(Sekolah s)
        {
            listSekolah.Add(s);
        }

        public void TambahPaket(PaketMBG p)
        {
            listPaket.Add(p);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("\n=== DATA PENGELOLAAN MBG ===");
            Console.WriteLine("Daftar Sekolah:");
            foreach (var s in listSekolah)
            {
                s.TampilInfo();
            }

            Console.WriteLine("Daftar Paket di Dapur:");
            foreach (var p in listPaket)
            {
                Console.WriteLine("ID: " + p.IdPaket + " | Nama: " + p.NamaPaket + " | Stok: " + p.StokDapur);
            }
            Console.WriteLine("===================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Sekolah s1 = new Sekolah("SCH01", "SD Negeri 1 Maju", 10);
            Sekolah s2 = new Sekolah("SCH02", "SMP Negeri 4 Sukamaju", 5);

            PaketMBG p1 = new PaketReguler("PKT1", "Reguler Standar", 100);
            PaketMBG p2 = new PaketKhusus("PKT2", "Khusus Diabetes", 50, "Rendah Gula");

            Console.WriteLine("=== CEK HITUNG BIAYA ===");
            Console.WriteLine("Biaya 10 Reguler: Rp " + p1.HitungBiaya(10));
            Console.WriteLine("Biaya 10 Khusus: Rp " + p2.HitungBiaya(10));
            Console.WriteLine("========================");

            DistribusiMBG distribusi = new DistribusiMBG(s1, p1, 20);
            distribusi.KirimPaket();

            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(s1);
            pengelola.TambahSekolah(s2);
            pengelola.TambahPaket(p1);
            pengelola.TambahPaket(p2);

            pengelola.TampilkanDataMBG();

            Console.ReadKey();
        }
    }
}

// Hubungan Aggregation: PengelolaMBG nampung list Sekolah dan PaketMBG
// Objek di dalam list dibikin di luar class ini dan bisa hidup sendiri walau pengelola dihapus.