using InventoryAPI.Data;
using InventoryAPI.DTO;
using InventoryAPI.Models;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Protocol.Plugins;

namespace InventoryAPI.Service
{
    public class SensorService : ISensorService
    {
        private readonly IConfiguration _configuration;
        private readonly InventoryContext _context;

        public SensorService(IConfiguration configuration, InventoryContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<Sensor> AddSensor(Sensor sensor)
        {
            try
            {
                _context.Sensors.Add(sensor);
                await _context.SaveChangesAsync();

                if (sensor.Temperature > 18)
                {
                    await SendEmailNotification(sensor);
                }

                return sensor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding sensor: {ex.Message}");
            }
        }
        public async Task<bool> UpdateSensor(int sensorId, SensorUpdateDto sensorUpdateDto)
        {
            try
            {
                var existingSensor = await _context.Sensors.FirstOrDefaultAsync(s => s.SensorId == sensorId);
                if (existingSensor == null)
                {
                    return false;
                }

                if (sensorUpdateDto.Temperature != null)
                {
                    existingSensor.Temperature = sensorUpdateDto.Temperature.Value;
                }
                if (sensorUpdateDto.Humidity != null)
                {
                    existingSensor.Humidity = sensorUpdateDto.Humidity.Value;
                }

                existingSensor.Timestamp = DateTime.UtcNow;

                bool needsNotification = existingSensor.Temperature < 15 ||
                                          existingSensor.Temperature > 25 ||
                                          existingSensor.Humidity < 40 ||
                                          existingSensor.Humidity > 60;

                if (needsNotification)
                {
                    await SendEmailNotification(existingSensor);
                }

                _context.Sensors.Update(existingSensor);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> DeleteSensor(int sensorId)
        {
            try
            {
                var sensorToDelete = await _context.Sensors.FindAsync(sensorId);
                if (sensorToDelete != null)
                {
                    _context.Sensors.Remove(sensorToDelete);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting sensor: {ex.Message}");
            }
        }

        private async Task SendEmailNotification(Sensor sensor)
        {
            try
            {
                var (email, password) = GetEmailSettings();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Inventory Company", email));
                message.To.Add(MailboxAddress.Parse("inna.posukansmi@gmail.com"));
                message.Subject = "High Temperature Alert";
                message.Body = new TextPart("plain")
                {
                    Text = $"The temperature inside has risen to {sensor.Temperature}°C. Sensor {sensor.SensorId} has malfunctioned. Please check the sensor status!"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(email, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email notification: {ex.Message}");
            }
        }
        public async Task<List<Sensor>> GetAllSensors()
        {
            return await _context.Sensors.ToListAsync();
        }
        private (string, string) GetEmailSettings()
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var email = emailSettings["Email"];
            var password = emailSettings["Password"];
            return (email, password);
        }
    }
}
