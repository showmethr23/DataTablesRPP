using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataTablesRPP.Models.ModelsPartials
{
    [ModelMetadataType(typeof(ProductsMetadata))]
    public class Products
    {
    }

    public class ProductsMetadata
    {
        [Range(0, 1000, ErrorMessage = "invalid stocklevel..")]
        public short? UnitsInStock { get; set; }
    }
}
