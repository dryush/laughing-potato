using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mail_bank.App
{
    public class NewProduct
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public NewProduct(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }


    public class ExistProduct
    {
        public long Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public ExistProduct(long id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
