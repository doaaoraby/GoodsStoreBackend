using GoodsStore.Models;
using System.Text;

namespace GoodsStore.Services
{
    public class GenerateFiles
    {
        public List<int> writeInFiles(List<Goods> lines)
        {
            List<int> goodsList = new List<int>();
            var csv = new StringBuilder();
            var outputFolder = Constants.Constants.outputFolder;
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            var dateRange = lines.OrderBy(x => x.TransactionDate).Select(x => x.TransactionDate).Distinct().ToList();
            lines = lines.OrderBy(x => x.GoodId).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0)
                {
                    csv.AppendLine(lines[i].GoodId + ";" + lines[i].TransactionId + ";" + lines[i].TransactionDate + ";" + lines[i].Amount + ";" + lines[i].Direction + ";" + lines[i].Comments);
                }
                else if (i == lines.Count - 1)
                {
                    csv.AppendLine(lines[i].GoodId + ";" + lines[i].TransactionId + ";" + lines[i].TransactionDate + ";" + lines[i].Amount + ";" + lines[i].Direction + ";" + lines[i].Comments);
                    File.WriteAllText(outputFolder + lines[i - 1].GoodId.ToString() + ".csv", csv.ToString());
                    goodsList.Add(lines[i - 1].GoodId);
                }
                else
                {
                    if (lines[i].GoodId == lines[i - 1].GoodId)
                    {
                        csv.AppendLine(lines[i].GoodId + ";" + lines[i].TransactionId + ";" + lines[i].TransactionDate + ";" + lines[i].Amount + ";" + lines[i].Direction + ";" + lines[i].Comments);
                    }
                    else
                    {
                        File.WriteAllText(outputFolder + lines[i - 1].GoodId.ToString() + ".csv", csv.ToString());
                        goodsList.Add(lines[i - 1].GoodId);
                        csv = new StringBuilder();
                        csv.AppendLine(lines[i].GoodId + ";" + lines[i].TransactionId + ";" + lines[i].TransactionDate + ";" + lines[i].Amount + ";" + lines[i].Direction + ";" + lines[i].Comments);
                    }
                }
            }
            return goodsList;
        }
    }
}
