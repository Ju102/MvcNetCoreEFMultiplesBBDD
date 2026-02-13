using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreEFMultiplesBBDD.Models
{
    [Table("V_EMPLEADOS")]
    public class Empleado
    {
        [Key]
        [Column("ID_EMPLEADO")]
        public int IdEmpleado { get; set; }

        [Column("APELLIDO")]
        public string Apellido { get; set; }

        [Column("OFICIO")]
        public string Oficio { get; set; }

        [Column("SALARIO")]
        public int Salario { get; set; }

        [Column("ID_DEPARTAMENTO")]
        public int IdDepartamento { get; set; }

        [Column("DEPARTAMENTO")]
        public string Departamento { get; set; }

        [Column("LOCALIDAD")]
        public string Localidad { get; set; }
    }
}
