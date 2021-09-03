using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImportAPIExcel.Model;
using System.IO;
using OfficeOpenXml;
using ImportAPIExcel.Data;
using Newtonsoft.Json;
using ClosedXML.Excel;

namespace ImportAPIExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ImportExcelContext _context;

        public ImportController(ImportExcelContext context)
        {
            _context = context;
        }

        // GET: api/Import
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExcelFile>>> GetAllImports()
        {
            try
            {
                var imports = await ImportDAO.GetAllImports(_context);

                if (imports == null)
                {
                    return NotFound();
                }

                return Ok(imports);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }

        // GET: api/Import/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExcelFile>> GetImportById(int id)
        {
            try
            {
                var imports = await ImportDAO.GetImportById(_context, id);

                if (imports == null)
                {
                    return NotFound();
                }

                return Ok(imports);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }




        // POST: api/Import
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExcelFile>> PostExcelFile(IFormFile excelFile)
        {

            List<ExcelFile> xList = new List<ExcelFile>();
            Lote lote;
            List<string> erros = new List<string>() { };

            try
            {

                if (excelFile == null)
                {
                    throw new Exception("Nenhum arquivo enviado");
                }


                using (var memoryStream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(memoryStream).ConfigureAwait(false);
                    var xls = new XLWorkbook(memoryStream);

                    DateTime DataEntrega = DateTime.Now;
                    string NomeProduto;
                    int Quantidade;
                    decimal ValorUnitario;
                    int linhaAtual = 0;

                    try
                    {


                        var planilha = xls.Worksheets.First(w => w.Name == "Planilha1");
                        var totalLinhas = planilha.Rows().Count();
                        
                        // primeira linha é o cabecalho
                        for (int l = 2; l < totalLinhas; l++)
                        {

                            linhaAtual = l;

                            try
                            {
                                DataEntrega = (DateTime)planilha.Cell($"A{l}").Value;
                                NomeProduto = planilha.Cell($"B{l}").Value.ToString();
                                Quantidade = Convert.ToInt32(planilha.Cell($"C{l}").Value.ToString());
                                ValorUnitario = Convert.ToDecimal(planilha.Cell($"D{l}").Value.ToString());

                                if (DataEntrega <= DateTime.Today)
                                {
                                    erros.Add($"Data inválida (Não pode ser menor ou igual a data atual) na linha: {l} coluna: 1");
                                }

                                if (NomeProduto.Length > 50)
                                {
                                    erros.Add($"A descrição deve conter no máximo 50 carácteres na linha: {l} coluna: 2");
                                }

                                if (Quantidade <= 0)
                                {
                                    erros.Add($"A quantidade deve ser um número válido(maior que 0(zero)) na linha: {l} coluna: 3");
                                }

                                if (ValorUnitario <= 0)
                                {
                                    erros.Add($"O valor deve ser um número válido(maior que 0(zero)) na linha: {l} coluna: 4");
                                }

                                xList.Add(new ExcelFile()
                                {
                                    DataEntrega = DataEntrega,
                                    NomeProduto = NomeProduto,
                                    Quantidade = Quantidade,
                                    ValorUnitario = ValorUnitario
                                });

                            }
                            catch (Exception ex)
                            {
                                erros.Add($"Erro no arquivo linha: {linhaAtual} -> {ex.Message}");
                                
                            }
                           


                            

                           
                        }
                    }
                    catch (Exception)
                    {
                        erros.Add($"Por favor, verifique todos os campos estão preenchidos corretamente na linha: {linhaAtual} ");
                    }


                    if (xList.Count <= 0)
                    {
                        erros.Add("Arquivo não importado. Verifique se existem erros no arquivo.");
                        throw new Exception();
                    }

                    if (erros.Count > 0)
                    {
                        erros.Add("Arquivo não importado. Verifique os erros.");
                        throw new Exception();
                    }

                }

            }
            catch (Exception)
            {
                return BadRequest(new { erros });
            }



            try
            {
                lote = new Lote() { DataImportacao = DateTime.Now, arquivos = xList };
                lote = await ImportDAO.Insert(_context, lote);
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
                return BadRequest(new { erros });
            }


            return Ok(new { lote.Id });
        }


        private bool ExcelFileExists(int id)
        {
            return _context.ExcelFiles.Any(e => e.Id == id);
        }
    }
}
