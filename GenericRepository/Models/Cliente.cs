namespace GenericRepository.Models
{
    public class Cliente : Persona
    {
        public Cliente()
        {
            Cuentas = new List<Cuenta>();
        }
        public string Contrasena { get; set; } = null!;
        public bool? Estado { get; set; }
        public virtual ICollection<Cuenta> Cuentas { get; set; }
    }
}
