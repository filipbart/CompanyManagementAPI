using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompanyManagementApi.Models
{
    public class Company
    {

        public class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //[JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EstablishmentDate { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
