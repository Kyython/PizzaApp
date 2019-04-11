using System;

namespace PizzaApp.Models.Abstract
{
    public abstract class Catalog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }
    }
}
