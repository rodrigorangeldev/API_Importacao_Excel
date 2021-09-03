using ImportAPIExcel.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImportAPIExcel.Data
{
    public static class ImportDAO
    {

        public async static Task<Lote> Insert(ImportExcelContext context, Lote lote)
        {

            using var transaction = context.Database.BeginTransaction();


            try
            {
                await context.Lote.AddAsync(lote);
                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();

                throw;
            }

            return lote;


        }

        public async static Task<object> GetAllImports(ImportExcelContext context)
        {
           var imports = await context.Lote.Include(x => x.arquivos).ToListAsync();

           var x = imports.Select(x => new
            {
                Id = x.Id,
                DataImportacao = x.DataImportacao.ToShortDateString(),
                NumeroItens = x.arquivos.Count,
                MenorData = x.arquivos.Select(a => a.DataEntrega).FirstOrDefault().ToShortDateString(),
                Total = x.arquivos.Sum(x => (x.Quantidade * x.ValorUnitario))
            });

            return x;

        }

        public async static Task<object> GetImportById(ImportExcelContext context, int id)
        {

            var query = from lote in context.Lote
                        join arquivo in context.ExcelFiles on lote equals arquivo.Lote 
                        where lote.Id == id
                        select new { 
                            arquivo.Id,
                            DataEntrega = arquivo.DataEntrega.ToShortDateString(),
                            arquivo.NomeProduto,
                            arquivo.Quantidade,
                            arquivo.ValorUnitario,
                            Total = arquivo.Quantidade * arquivo.ValorUnitario
                        };

            //var imports = context.Lote.Include(x => x.arquivos && x.Id == id).ToList();
            //var files = imports.Select(x => x.arquivos).ToList();

            //var filesFiltered = files.Select(x => new
            //{
            //    id = x.Select(a => a.Id),
            //    dataEntrega = x.Select(a => a.DataEntrega.ToShortDateString()),
            //    quantidade = x.Select(a => a.Quantidade),
            //    valorUtinario = x.Select(a => a.ValorUnitario),
            //    Total = x.Select(a => (a.Quantidade * a.ValorUnitario))
            //}).ToList();


            return await query.ToListAsync();


        }


    }
}
