namespace Api;

public class PlatoUpdateDTO
{
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public uint TiempoPreparacion { get; set; }
        public string? ImagenUrl { get; set; }
        public bool Disponible { get; set; }
        public bool EsMenuDelDia { get; set; }
}
