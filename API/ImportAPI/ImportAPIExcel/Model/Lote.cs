using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImportAPIExcel.Model
{
    public class Lote
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataImportacao { get; set; }

        [ForeignKey("LoteRefId")]
        public ICollection<ExcelFile> arquivos { get; set; }
    }
}
