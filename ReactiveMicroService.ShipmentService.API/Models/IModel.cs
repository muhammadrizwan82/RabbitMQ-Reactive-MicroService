namespace ReactiveMicroService.ShipmentService.API.Models
{
    public interface IModel
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedIP { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedIP { get; set; }
    }
}
