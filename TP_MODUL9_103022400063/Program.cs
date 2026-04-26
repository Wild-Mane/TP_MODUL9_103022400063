using System.Text.Json;
using System.Text.Json.Serialization;

class CovidConfig
{
    public string satuan_suhu { get; set; }
    public double batas_hari_demam { get; set; }
    public string pesan_ditolak { get; set; }
    public string pesan_diterima { get; set; }

    public CovidConfig()
    {
        readJSON();
    }

    public void readJSON()
    {
        string filePath = "config.json";
        CovidConfig config;
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            config = JsonSerializer.Deserialize<CovidConfig>(jsonString);
            this.satuan_suhu = config.satuan_suhu;
            this.batas_hari_demam = config.batas_hari_demam;
            this.pesan_ditolak = config.pesan_ditolak;
            this.pesan_diterima = config.pesan_diterima;
        }
        else
        {

            this.satuan_suhu = "fahrenheit";
            this.batas_hari_demam = 14;
            this.pesan_ditolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
            this.pesan_diterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";

        }
    }
    public void ubahSatuan()
    {
        if (string.Equals(satuan_suhu, "celsius", StringComparison.OrdinalIgnoreCase))
        {
            satuan_suhu = "fahrenheit";
        }
        else if (string.Equals(satuan_suhu, "fahrenheit", StringComparison.OrdinalIgnoreCase))
        {
            satuan_suhu = "celsius";
        }
    }
    public double CelciusKeFahrenheit(double suhuCelcius)
    {
        return (suhuCelcius * 9.0 / 5.0) + 32.0;
    }

    public double FahrenheitKeCelcius(double suhuFahrenheit)
    {
        return (suhuFahrenheit - 32.0) * 5.0 / 9.0;
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        CovidConfig config = new CovidConfig();

        Console.WriteLine("================= Sistem Pemeriksaan Suhu Badan untuk Akses Gedung =================\n");
        Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.satuan_suhu}");
        double suhu = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
        int hari = Convert.ToInt32(Console.ReadLine());

        bool isSuhuAman = false;

        if (config.satuan_suhu.ToLower() == "celsius")
        {
            isSuhuAman = (suhu >= 36.5 && suhu <= 37.5);
        }
        else if (config.satuan_suhu.ToLower() == "fahrenheit")
        {
            isSuhuAman = (suhu >= 97.7 && suhu <= 99.5);
        }

        bool isHariAman = (hari < config.batas_hari_demam);

        if (isSuhuAman && isHariAman)
        {
            Console.WriteLine(config.pesan_diterima);
        }
        else
        {
            Console.WriteLine(config.pesan_ditolak);
        }

        string satuanLama = config.satuan_suhu.ToLower();
        config.ubahSatuan();
        Console.WriteLine("\n=================[Info] Satuan suhu telah diubah.=================");
        Console.WriteLine($"\n[Info] Satuan suhu sekarang diubah menjadi: {config.satuan_suhu}");

        double suhuBaru = suhu;
        if (satuanLama == "celsius" && config.satuan_suhu.ToLower() == "fahrenheit")
        {
            suhuBaru = config.CelciusKeFahrenheit(suhu);
        }
        else if (satuanLama == "fahrenheit" && config.satuan_suhu.ToLower() == "celsius")
        {
            suhuBaru = config.FahrenheitKeCelcius(suhu);
        }

        Console.WriteLine($"\n[Info] Nilai suhu yang sebelumnya {suhu} {satuanLama} sekarang menjadi {suhuBaru} {config.satuan_suhu}");
    }
}