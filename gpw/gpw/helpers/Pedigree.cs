using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gpw
{
    public static class Pedigree
    {
        public static void render()
        {

            var data = new List<arrPedigree>() { 
                new arrPedigree() { name = "Parrent", children = new arrPedigree() { name = "child 1" } },
                new arrPedigree() { name = "" }
            };

            
        }
    }

    public class arrPedigree
    {
        public string name { get; set; }
        public arrPedigree children { get; set; }
    }
}