using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImportAPIExcel.Model
{
    public class ExcelFile
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime DataEntrega {get; set;}

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string NomeProduto { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ValorUnitario { get; set; }

        [Required]
        public Lote Lote { get; set; }
    }
}
