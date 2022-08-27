using CsvHelper;
using CsvHelper.Configuration;
using GoodsStore.Models;
using System.Globalization;
using System.Text;

namespace GoodsStore.Services
{
    public class ReadFile
    {
        public List<Goods> readInputFile(StreamReader sReader)
        {
            List<Goods> records = new List<Goods>();
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = false
            };
            using (var csvReader = new CsvReader(sReader, csvConfig))
            {
                records = csvReader.GetRecords<Goods>().ToList();
                records.RemoveAll(x => x.GoodId == 0 || x.TransactionId == 0 || x.TransactionDate == "" || x.Amount == "" || x.Direction == "");
            }
            Console.WriteLine(records);
         
            return records;
        }
        public (List<FirstBalance>, StreamReader) readFirstBalance(StreamReader sReader)
        {
            string line = sReader.ReadLine();
            int countLines = 0;
            List<FirstBalance> frstBlnceList = new List<FirstBalance>();
            while (!line.StartsWith(";;;"))
            {
                var values = line.Split(';');
                var frstBlnce = new FirstBalance();
                Console.WriteLine((values[1].Split(' '))[1]);
                frstBlnce.GoodId = Int32.Parse((values[1].Split(' '))[1]);
                frstBlnce.FrstBalance = Int32.Parse(values[0]);
                frstBlnceList.Add(frstBlnce);
                line = sReader.ReadLine();
                countLines++;
            }
            return (frstBlnceList, sReader);
        }

        public StreamReader readSeparationLines(StreamReader sReader)//to escape line separation
        {
            string line;
            do
            {
                line = sReader.ReadLine();
            }
            while (line.StartsWith(";;;"));
            return sReader;
        }
        public void writeInLogFile(List<Goods> removedGoods)
        {
            var logFoLder = Constants.Constants.logFolder;
            if (!Directory.Exists(logFoLder))
            {
                Directory.CreateDirectory(logFoLder);
            }
            var logFile = new StringBuilder();
            foreach (Goods item in removedGoods)
            {
                logFile.AppendLine(item.GoodId + ";" + item.TransactionId + ";" + item.TransactionDate + ";" + item.Amount + ";" + item.Direction + ";" + item.Comments);
            }
            File.WriteAllText(logFoLder + "error.log", logFile.ToString());
        }
    }
}
