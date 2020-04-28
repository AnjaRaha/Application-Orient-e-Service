using StockManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillManager
{
    public class BillLine
    {
        public Item item { get; set; }

        public int quantite { get; set; }

        public double sousTotal { get; set; }

        public BillLine(Item item , int quantite)
        {
            this.item = item;
            this.quantite = quantite;
            this.sousTotal = item.prix * quantite;

        }

        
        
    }
}
