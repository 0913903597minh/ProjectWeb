using LibData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibData.Utilities
{
    public class RamdomColor
    {

        public static string getColor(string data)
        {
            var srcs = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<ProportionValue> listsrc = new List<ProportionValue>();
            foreach (var item in srcs)
            {
                ProportionValue model = new ProportionValue();
                if (!string.IsNullOrEmpty(item))
                {
                    var srcandratio = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    model.Proportion = Convert.ToDouble(srcandratio[1]);
                    model.Value = srcandratio[0];
                    listsrc.Add(model);
                }
            }
            return ChooseByRandom(listsrc);
        }
       
        public static string ChooseByRandom(
           List<ProportionValue> collection)
        {
            Random random = new Random();
            var rnd = random.NextDouble();

            foreach (var item in collection)
            {
                if (rnd < item.Proportion)
                    return item.Value;
                rnd -= item.Proportion;
            }
            throw new InvalidOperationException(
                //The proportions in the collection do not add up to 1.
                "");
        }
    }
}
