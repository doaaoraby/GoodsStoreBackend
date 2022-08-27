using GoodsStore.Models;
using GoodsStore.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using GoodsStore.Constants;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;

namespace GoodsStore.Controllers
{
   

    public class GoodsController
    {
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Route("[action]")]
        
        public async Task<List<int>> PostFile(IFormFile file)
        {
            try
            {
                var obj = new ReturnedData();
                var outputFolder = Constants.Constants.outputFolder;
                var inputFolder = Constants.Constants.inputFolder;
                var generateFiles = new GenerateFiles();
                var readFile = new ReadFile();
                List<int> goodsIDs = new List<int>();
                List<string>dateRange=new List<string>();
                var fileName = file.FileName;
                if (!Directory.Exists(inputFolder)) { Directory.CreateDirectory(inputFolder); }
                var path = Path.Combine(inputFolder, fileName);
                if (!System.IO.File.Exists(path))
                {
                    using (var stream = System.IO.File.Create(path)) { }
                }
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }
                System.IO.File.WriteAllBytes(path, bytes);
                using (var reader = new StreamReader(path))
                {
                    reader.ReadLine();
                    reader.ReadLine();
                    (List<FirstBalance> fb, StreamReader sr) = readFile.readFirstBalance(reader);
                    StreamReader sRead = readFile.readSeparationLines(sr);
                    List<Goods> readLines = readFile.readInputFile(sRead);
                     goodsIDs= generateFiles.writeInFiles(readLines);
                }
                return goodsIDs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Goods>> GetData(int goodId, string fromDate, string toDate)
        {
            try 
            { 
                List<Goods> records = new List<Goods>();
                if (fromDate != null && toDate != null)
                { 
                    
                }
                    using (var reader = new StreamReader(Constants.Constants.outputFolder + goodId + ".csv"))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";",
                        HasHeaderRecord = false
                    };
                    using (var csvReader = new CsvReader(reader, csvConfig))
                    {
                        DateTime fDate;
                        DateTime tDate;
                        records = csvReader.GetRecords<Goods>().ToList();
                        if (fromDate != null && toDate != null)
                        {
                            
                            fDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            tDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            var r=records.FirstOrDefault();
                            records=records.Where(x=> DateTime.ParseExact(x.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= fDate
                            && DateTime.ParseExact(x.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= tDate).ToList();
                        }
                        else
                        {
                            if (toDate == null)
                            {
                                fDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                records = records.Where(x => DateTime.ParseExact(x.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= fDate).ToList();
                            }
                            else if (fromDate == null)
                            {
                                tDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                records = records.Where(x => DateTime.ParseExact(x.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= tDate).ToList();
                            }
                            
                        }
                    }
                }
                if (records.Count() == 0)
                {
                    throw new Exception();
                }
                else 
                { return records; }
            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
