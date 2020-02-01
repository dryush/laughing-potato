using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailBank.App
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
        // Открыл поля для записи в файл
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Для десириализации
        private ExistProduct() { }
        public ExistProduct(long id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
