using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using System.Linq;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // Busca a tarefa pelo Id
            var tarefa = _context.Tarefas.Find(id);

            if (tarefa == null)
                return NotFound(); // retorna 404 se não encontrar

            return Ok(tarefa); // retorna 200 com o objeto
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // Busca todas as tarefas
            var tarefas = _context.Tarefas.ToList();
            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // Busca tarefas que contenham o título
            var tarefas = _context.Tarefas
                                  .Where(x => x.Titulo.Contains(titulo))
                                  .ToList();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            // Busca tarefas pela data (comparando só a parte de Data)
            var tarefas = _context.Tarefas
                                  .Where(x => x.Data.Date == data.Date)
                                  .ToList();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // Busca tarefas pelo Status
            var tarefas = _context.Tarefas
                                  .Where(x => x.Status == status)
                                  .ToList();

            return Ok(tarefas);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Adiciona no EF e salva
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // Atualiza os campos
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // Atualiza no EF e salva
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Remove do EF e salva
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent(); // 204
        }
    }
}
