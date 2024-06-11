using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly string _connectionString;

        public BackupController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection"); 
        }

        [HttpPost("CreateBackup")]
        public IActionResult CreateBackup()
        {
            try
            {
                string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
                string backupFileName = $"DatabaseBackup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string backupFilePath = Path.Combine(backupPath, backupFileName);

                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                string sqlCommand = $"BACKUP DATABASE Inventory TO DISK = '{backupFilePath}'";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(backupFilePath);
                return File(fileBytes, "application/octet-stream", backupFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ошибка при создании резервной копии: {ex.Message}");
            }
        }

    }
}
