using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportAPIExcel.Model
{
    public class ImportExcelContext: DbContext
    {
        
            public DbSet<ExcelFile> ExcelFiles { get; set; }
            public DbSet<Lote> Lote { get; set; }

        public ImportExcelContext(DbContextOptions<ImportExcelContext> options)
                : base(options)
            { }
        


    }
}
