using System;
using System.Collections.Generic;
using System.Text;

namespace StockManager
{
    public class ItemLine
    {
        public Item item { get; set; }
        public int quantity { get; set; }

        public ItemLine(Item item , int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public ItemLine()
        {

        }
    }
}
