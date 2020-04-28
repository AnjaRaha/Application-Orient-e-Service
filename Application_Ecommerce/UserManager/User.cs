using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;




namespace UserManager
{
    public class User
    {
        public string userName { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string email { get; set; }

        public string mdp { get; set; }
        

        public User(string userName, string nom, string prenom, string email, string mdp)
        {
            this.userName = userName;
            this.nom = nom;
            this.prenom = prenom;
            this.email = email;
            this.mdp = mdp;
        }

        public User()
        {

        }

        public string GetUser(string nom)
        {

            bool trouver = false;
            List<User> listeUser = new List<User>();
            //chargement des données Json (fichier contenant les utilisateurs)
            using (System.IO.StreamReader r = new System.IO.StreamReader("Users.json"))
            {

                string json = r.ReadToEnd();
                listeUser = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(json);

            }

            //authentification de l'utilisateur
            foreach (User utilisateur in listeUser)
            {
                if (nom == utilisateur.userName)
                {
                    trouver = true;
                }
            }
            if (trouver == true) { Console.WriteLine("Authentification réussie"); }
            else { Console.WriteLine("Introuvable"); }
            return trouver.ToString();


        }

        }
}
