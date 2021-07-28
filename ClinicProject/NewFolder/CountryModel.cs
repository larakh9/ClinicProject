using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.NewFolder
{
    public class CountryModel
    {
        public CountryModel()
        {
          
        }

        public CountryModel(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }



        //country name
        public string Name { get; set; }
    }

}

   

