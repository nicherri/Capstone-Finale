﻿namespace TravelEasy.Models.DTO
{
    public class ShelfDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ShelvingId { get; set; }
        public bool IsOccupied { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();


    }

}
