namespace DataTablesRPP.Models.ViewModels
{
    public partial class ProductEditVM : Products
    {
        public ProductEditVM() { }
        public ProductEditVM(Products prod, string msg)
        {
            this.ProductId = prod.ProductId;
            this.ProductName = prod.ProductName;
            this.UnitPrice = prod.UnitPrice;
            this.UnitsInStock = prod.UnitsInStock;
            this.Message = msg;
        }
        public string Message { get; set; }

    }
}
