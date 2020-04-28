using System;
using System.Collections.Generic;
using System.Text;

namespace StockManager
{
    public class Item
    {
        public string nom { get; set; }
        public double prix { get; set; }

        public Item(string nom, double prix)
        {
            this.nom = nom;
            this.prix = prix;
        }

        
        
    }

    
}
