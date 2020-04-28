using System;
using System.Collections.Generic;
using System.Text;
using UserManager;

namespace BillManager
{
   public class Bill
    {
        public User utilisateur;
        public List<BillLine> listeBill=new List<BillLine>();
        public double sousTotalSansTaxe;
        public double sousTotalAvecTaxe;

        public Bill(User utilisateur, List<BillLine>listeBill, double sousTotalSansTaxe, double sousTotalAvecTaxe)
        {
            this.utilisateur = utilisateur;
            this.listeBill = listeBill;
            this.sousTotalSansTaxe = sousTotalSansTaxe;
            this.sousTotalAvecTaxe = sousTotalAvecTaxe;
        }

        public Bill()
        {

        }

        public static Bill CreateBill(User user, List<StockManager.ItemLine> lines)
        {
            Bill facture = new Bill();
            facture.utilisateur = user;
            foreach(StockManager.ItemLine item in lines)
            {
                facture.listeBill.Add(new BillLine(item.item, item.quantity));
            }
            double sousTotal =0; 
            foreach(BillLine b in facture.listeBill)
            {
                sousTotal += b.sousTotal;
            }
            facture.sousTotalSansTaxe = sousTotal;
            facture.sousTotalAvecTaxe = sousTotal + (sousTotal * 0.14975);

            return facture;
        }

        public void afficher()
        {
            Console.WriteLine("Facture Total: ");
            Console.WriteLine("Client :" + this.utilisateur.nom);
            Console.WriteLine("Produit:");
            foreach(BillLine b in listeBill)
            {
                Console.WriteLine(b.item.nom + " " + b.item.prix + "$" );
            }
            Console.WriteLine("Total sans taxe : " + sousTotalSansTaxe.ToString());
            Console.WriteLine("Total avec Taxe: "+sousTotalAvecTaxe.ToString());
            
        }

        

    }
}
