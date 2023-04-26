using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AsciiArt;

namespace JeuDuPendu
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres)
        {
            for(int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            foreach (var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }

            if(mot == "")
            {
                return true;
            }
            return false;
        }

        static char DemanderUneLettre(string message = "Entrer une lettre svp : ")
        {
            while (true)
            {
                Console.Write(message);
                string lettre = Console.ReadLine();
                if(lettre.Length == 1)
                {
                    lettre = lettre.ToUpper();
                    return lettre[0];
                }
                Console.WriteLine("ERREUR : vous devez rentrer une lettre");
            }
        }

        static void DevinerMot(string mot)
        {
            var lettresDevinees = new List<char>();
            var lettresHorsDuMot = new List<char>();
            const int NB_VIES = 6;
            int vieRestantes = NB_VIES;


            while (vieRestantes != 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - vieRestantes]);
                Console.WriteLine();

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                
                char lettre = DemanderUneLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Cette lettre est dans le mot");
                    lettresDevinees.Add(lettre);
                    if(ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        Console.WriteLine("BRAVO ! Vous avez gagné");
                        break;
                    }
                }
                else
                {
                    if (!lettresHorsDuMot.Contains(lettre))
                    {
                        lettresHorsDuMot.Add(lettre);
                        vieRestantes--;
                    }
                    Console.WriteLine($"Il vous reste {vieRestantes}");
                }
                if (lettresHorsDuMot.Count > 0)
                {
                    Console.WriteLine("Le mot ne contient pas : " + String.Join(", ", lettresHorsDuMot));
                }
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - vieRestantes]);
            if (vieRestantes == 0)
            {
                Console.WriteLine($"PERDU ! Le mot était {mot}");
            }
        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch(Exception e)
            {
                Console.WriteLine("ERREUR : le fichier n'est pas trouvé.\n(" + e.Message + ")");
            }
            return null;
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");
            char rejouer;
            if (mots == null || mots.Length == 0)
            {
                Console.WriteLine("Le fichier est vide");
            }
            else
            {
                do
                {
                    Console.Clear();
                    Random r = new Random();
                    int ligneFichier = r.Next(0, mots.Length);
                    string mot = mots[ligneFichier].Trim().ToUpper();
                    DevinerMot(mot);
                    do
                    {
                        rejouer = DemanderUneLettre("Voulez vous rejouer ? [O/N] : ");
                    } while (rejouer != 'O' && rejouer != 'N');
                } while (rejouer == 'O');
                if(rejouer == 'N')
                {
                    Console.WriteLine(@"
* * * * * * * * * * * * * * *
* MERCI DE TA PARTICIPATION *
* * * * * * * * * * * * * * *
");
                }
            }
            /*AfficherMot(mot, new List<char> { });
            char lettre = DemanderUneLettre();
            AfficherMot(mot, new List<char> { lettre });*/
        }
    }
}
