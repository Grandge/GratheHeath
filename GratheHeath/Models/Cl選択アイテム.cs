using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GratheHeath.Models
{
    public class C選択アイテム
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public C選択アイテム()
        {
            Name = "";
            Code = "";
        }
        public C選択アイテム(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
