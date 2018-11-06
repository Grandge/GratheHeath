using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GratheHeath.Models;

namespace GratheHeath.ViewModels
{

    public class VM選択項目
    {
        public VM選択項目()
        {
            ListItem = new ObservableCollection<C選択アイテム>();
        }
        public ObservableCollection<C選択アイテム> ListItem { get; set; }
    }
}
