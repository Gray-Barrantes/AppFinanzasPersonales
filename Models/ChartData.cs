namespace ProyectoGerencia.Models
{
    public class ChartData
    {
        public string Label { get; set; }
        public decimal Value { get; set; }

        // Constructor para inicializar las propiedades con valores predeterminados
        public ChartData(string label = "", decimal value = 0)
        {
            Label = label;
            Value = value;
        }
    }
}
