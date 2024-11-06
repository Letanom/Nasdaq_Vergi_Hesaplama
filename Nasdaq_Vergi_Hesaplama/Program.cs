using System;
using System.Globalization;
using System.Threading.Tasks;

public class StockCalculator
{
    public async Task<decimal> CalculateTotalGain(decimal purchasePriceUSD, decimal salePriceUSD, DateTime purchaseDate, DateTime saleDate, decimal purchaseExchangeRate, decimal saleExchangeRate, decimal purchaseYufeIndex, decimal saleYufeIndex)
    {
        
        // Alış ve satış fiyatlarını Türk Lirası'na çevir
        decimal purchasePriceTRY = purchasePriceUSD * purchaseExchangeRate;
        decimal salePriceTRY = salePriceUSD * saleExchangeRate;

        // Yİ-ÜFE farkına göre endeksleme yapılacaksa
        decimal gainTRY;
        decimal yufeDifference = (saleYufeIndex - purchaseYufeIndex) / purchaseYufeIndex * 100;

        if (yufeDifference > 10)
        {
            decimal indexedPurchasePrice = purchasePriceTRY * (saleYufeIndex / purchaseYufeIndex);
            gainTRY = salePriceTRY - indexedPurchasePrice;
        }
        else
        {
            gainTRY = salePriceTRY - purchasePriceTRY;
        }

        return gainTRY;
    }
}

public class Program
{

    public static async Task Main(string[] args)
    {

        // Konsol arka plan rengini ayarla
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("** STOK SATIŞ KAZANCI HESAPLAMA ARACI **");
        Console.WriteLine("Bu program, ABD borsasındaki işlemlerden elde edilen kazançları hesaplar.");
        Console.WriteLine("BU UYGULAMA KEVIN ÖZŞİMŞEK TARAFINDAN - YAPILMIŞTIR HİÇ BİR HUKUKİ SORUMLULUĞU VE RİSKİ KABUL ETMEZ.");
        Console.WriteLine("UYGULAMAYI KULLANIRKEN KENDİ VERİLERİNİZİ GİRİNİZ.");
        Console.WriteLine("BU SADECE BİR YAZILIM PROJESİDİR.");
        Console.WriteLine("*******************************************************************************************************************");
        Console.WriteLine();    
        Console.WriteLine();

        StockCalculator calculator = new StockCalculator();

        // Kullanıcıdan verileri al
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("Alış fiyatı (USD): ");
        decimal purchasePriceUSD = GetDecimalInput();

        Console.WriteLine("Satış fiyatı (USD): ");
        decimal salePriceUSD = GetDecimalInput();

        Console.WriteLine("Alış tarihi (yyyy-MM-dd formatında girin): ");
        DateTime purchaseDate = GetDateInput();

        Console.WriteLine("Satış tarihi (yyyy-MM-dd formatında girin): ");
        DateTime saleDate = GetDateInput();

        // Döviz kuru verilerini manuel girme
        Console.WriteLine("Alış tarihi için döviz kuru (USD/TRY): ");
        decimal purchaseExchangeRate = GetDecimalInput();

        Console.WriteLine("Satış tarihi için döviz kuru (USD/TRY): ");
        decimal saleExchangeRate = GetDecimalInput();

        // Yİ-ÜFE verilerini manuel girme
        Console.WriteLine("Alış tarihi Yİ-ÜFE endeksi: ");
        decimal purchaseYufeIndex = GetDecimalInput();

        Console.WriteLine("Satış tarihi Yİ-ÜFE endeksi: ");
        decimal saleYufeIndex = GetDecimalInput();

        // Kazancı hesapla
        decimal taxableGain = await calculator.CalculateTotalGain(purchasePriceUSD, salePriceUSD, purchaseDate, saleDate, purchaseExchangeRate, saleExchangeRate, purchaseYufeIndex, saleYufeIndex);

        // Vergiyi hesapla
        decimal taxRate = 0.15M; // %15 vergi oranı
        decimal taxAmount = taxableGain * taxRate;

        // Hesaplanan kazancı ve vergiyi yazdır
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nVergiye tabi kazanç (TL): {taxableGain:F2}");
        Console.WriteLine($"Ödenecek vergi (TL): {taxAmount:F2}");
        Console.ResetColor();
    }

    // Kullanıcıdan decimal veri al
    public static decimal GetDecimalInput()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal value))
            {
                return value;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Geçersiz giriş! Lütfen geçerli bir sayı girin.");
                Console.ResetColor();
            }
        }
    }

    // Kullanıcıdan tarih verisi al
    public static DateTime GetDateInput()
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Geçersiz tarih formatı! Lütfen 'yyyy-MM-dd' formatında bir tarih girin.");
                Console.ResetColor();
            }
        }
    }
}
