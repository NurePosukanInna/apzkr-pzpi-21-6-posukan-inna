using InventoryAPI.DTO;
using InventoryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InventoryAPI.Service
{
    public interface ISensorService

    {
        Task<List<Sensor>> GetAllSensors();

        Task<Sensor> AddSensor(Sensor sensor);
        Task<bool> DeleteSensor(int sensorId);
        Task<bool> UpdateSensor(int sensorId, SensorUpdateDto sensorUpdateDto);

    }
}
