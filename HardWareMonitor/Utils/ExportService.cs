using HardWareMonitor.Models;
using HardWareMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace HardWareMonitor.Utils
{
    public class ExportService
    {
        // Экспортирует данные в текстовый файл с форматированным выводом
        public void ExportToTxt(string filePath, object data)
        {
            StringBuilder sb = new StringBuilder();
            AppendData(sb, data);
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        // Экспортирует данные в CSV файл с разделителями точка с запятой
        public void ExportToCsv(string filePath, object data)
        {
            StringBuilder sb = new StringBuilder();

            if (data is MainViewModel vm)
            {
                sb.AppendLine("=== СИСТЕМНАЯ ИНФОРМАЦИЯ ===");
                sb.AppendLine($"Компьютер;{vm.SystemInfo.MachineName}");
                sb.AppendLine($"ОС;{vm.SystemInfo.OSName}");
                sb.AppendLine($"Версия;{vm.SystemInfo.OSVersion}");
                sb.AppendLine();

                sb.AppendLine("=== ПРОЦЕССОР ===");
                sb.AppendLine($"Модель;{vm.CpuInfo.Name}");
                sb.AppendLine($"Производитель;{vm.CpuInfo.Manufacturer}");
                sb.AppendLine($"Архитектура;{vm.CpuInfo.Architecture}");
                sb.AppendLine($"Ядра;{vm.CpuInfo.CoreCount}");
                sb.AppendLine($"Потоки;{vm.CpuInfo.ThreadCount}");
                sb.AppendLine($"Частота;{vm.CpuInfo.BaseFrequency} МГц");
                sb.AppendLine($"Загрузка;{vm.CpuInfo.LoadPercentage:F1}%");
                sb.AppendLine();

                sb.AppendLine("=== ПАМЯТЬ ===");
                sb.AppendLine($"Всего;{vm.MemoryInfo.TotalMemoryBytes} байт");
                sb.AppendLine($"Доступно;{vm.MemoryInfo.AvailableMemoryBytes} байт");
                sb.AppendLine($"Использование;{vm.MemoryInfo.UsagePercentage:F1}%");
                sb.AppendLine($"Количество модулей;{vm.MemoryInfo.Modules.Count}");

                foreach (var module in vm.MemoryInfo.Modules)
                {
                    sb.AppendLine($"Модуль;{module.Manufacturer};{module.Capacity} байт;{module.Speed} МГц");
                }
                sb.AppendLine();

                sb.AppendLine("=== ФИЗИЧЕСКИЕ ДИСКИ ===");
                sb.AppendLine("Модель;Размер;Тип");
                foreach (var disk in vm.DiskInfo.PhysicalDisks)
                {
                    sb.AppendLine($"{disk.Model};{disk.Size} байт;{disk.MediaType}");
                }
                sb.AppendLine();

                sb.AppendLine("=== ЛОГИЧЕСКИЕ ДИСКИ ===");
                sb.AppendLine("Диск;Общий размер;Свободно;Файловая система;Заполнение");
                foreach (var disk in vm.DiskInfo.LogicalDisks)
                {
                    sb.AppendLine($"{disk.DriveLetter};{disk.TotalSize} байт;{disk.FreeSpace} байт;{disk.FileSystem};{disk.UsagePercentage:F1}%");
                }
                sb.AppendLine();

                sb.AppendLine("=== ВИДЕОКАРТА ===");
                sb.AppendLine($"Модель;{vm.GpuInfo.Name}");
                sb.AppendLine($"Видеопамять;{vm.GpuInfo.VideoMemory} байт");
                sb.AppendLine($"Загрузка GPU;{vm.GpuInfo.LoadPercentage:F1}%");
                sb.AppendLine();

                sb.AppendLine("=== СЕТЬ ===");
                sb.AppendLine($"Адаптер;{vm.NetworkInfo.Name}");
                sb.AppendLine($"MAC-адрес;{vm.NetworkInfo.MacAddress}");
                sb.AppendLine($"Скорость;{vm.NetworkInfo.Speed} бит/с");
                sb.AppendLine();

                sb.AppendLine("=== ТОП-20 ПРОЦЕССОВ ===");
                sb.AppendLine("ID;Имя;Память (байт)");
                foreach (var process in vm.SystemInfo.Processes)
                {
                    sb.AppendLine($"{process.Id};{process.Name};{process.WorkingSet}");
                }
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        // Экспортирует данные в JSON файл с форматированием
        public void ExportToJson(string filePath, object data)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json, Encoding.UTF8);
        }

        // Форматирует данные для вывода в текстовый файл с красивым оформлением
        private void AppendData(StringBuilder sb, object data)
        {
            if (data is MainViewModel vm)
            {
                sb.AppendLine("========================================");
                sb.AppendLine("     ОТЧЕТ HARDWARE MONITOR PRO");
                sb.AppendLine("========================================");
                sb.AppendLine($"Дата отчета: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                sb.AppendLine($"Компьютер: {vm.SystemInfo.MachineName}");
                sb.AppendLine("========================================");
                sb.AppendLine();

                sb.AppendLine("ИНФОРМАЦИЯ О ПРОЦЕССОРЕ");
                sb.AppendLine("------------------------");
                sb.AppendLine($"  Модель: {vm.CpuInfo.Name}");
                sb.AppendLine($"  Производитель: {vm.CpuInfo.Manufacturer}");
                sb.AppendLine($"  Архитектура: {vm.CpuInfo.Architecture}");
                sb.AppendLine($"  Количество ядер: {vm.CpuInfo.CoreCount}");
                sb.AppendLine($"  Количество потоков: {vm.CpuInfo.ThreadCount}");
                sb.AppendLine($"  Базовая частота: {vm.CpuInfo.BaseFrequency} МГц");
                sb.AppendLine($"  Текущая загрузка: {vm.CpuInfo.LoadPercentage:F1}%");
                sb.AppendLine();

                sb.AppendLine("ИНФОРМАЦИЯ О ПАМЯТИ");
                sb.AppendLine("-------------------");
                sb.AppendLine($"  Всего оперативной памяти: {vm.MemoryInfo.TotalMemoryBytes:N0} байт");
                sb.AppendLine($"  Доступно памяти: {vm.MemoryInfo.AvailableMemoryBytes:N0} байт");
                sb.AppendLine($"  Использование памяти: {vm.MemoryInfo.UsagePercentage:F1}%");
                sb.AppendLine($"  Количество модулей памяти: {vm.MemoryInfo.Modules.Count}");

                if (vm.MemoryInfo.Modules.Count > 0)
                {
                    sb.AppendLine("  Модули памяти:");
                    foreach (var module in vm.MemoryInfo.Modules)
                    {
                        sb.AppendLine($"    - {module.Manufacturer} {module.Capacity:N0} байт {module.Speed} МГц");
                    }
                }
                sb.AppendLine();

                sb.AppendLine("ИНФОРМАЦИЯ О ДИСКАХ");
                sb.AppendLine("-------------------");
                sb.AppendLine("  Физические диски:");
                foreach (var disk in vm.DiskInfo.PhysicalDisks)
                {
                    sb.AppendLine($"    - {disk.Model} ({disk.Size:N0} байт) - {disk.MediaType}");
                }
                sb.AppendLine("  Логические диски:");
                foreach (var disk in vm.DiskInfo.LogicalDisks)
                {
                    sb.AppendLine($"    - {disk.DriveLetter}: Всего: {disk.TotalSize:N0} байт, Свободно: {disk.FreeSpace:N0} байт ({disk.UsagePercentage:F1}% занято) - {disk.FileSystem}");
                }
                sb.AppendLine();

                sb.AppendLine("ИНФОРМАЦИЯ О ВИДЕОКАРТЕ");
                sb.AppendLine("----------------------");
                sb.AppendLine($"  Модель: {vm.GpuInfo.Name}");
                sb.AppendLine($"  Объем видеопамяти: {vm.GpuInfo.VideoMemory:N0} байт");
                sb.AppendLine($"  Загрузка GPU: {vm.GpuInfo.LoadPercentage:F1}%");
                sb.AppendLine();

                sb.AppendLine("ИНФОРМАЦИЯ О СЕТИ");
                sb.AppendLine("-----------------");
                sb.AppendLine($"  Сетевой адаптер: {vm.NetworkInfo.Name}");
                sb.AppendLine($"  MAC-адрес: {vm.NetworkInfo.MacAddress}");
                sb.AppendLine($"  Скорость: {vm.NetworkInfo.Speed:N0} бит/с");
                sb.AppendLine();

                sb.AppendLine("ТОП-20 АКТИВНЫХ ПРОЦЕССОВ");
                sb.AppendLine("-------------------------");
                sb.AppendLine("  ID\tИмя процесса\t\tПамять (байт)");
                foreach (var process in vm.SystemInfo.Processes.Take(20))
                {
                    sb.AppendLine($"  {process.Id}\t{process.Name}\t\t{process.WorkingSet:N0}");
                }
                sb.AppendLine();
                sb.AppendLine("========================================");
                sb.AppendLine("           КОНЕЦ ОТЧЕТА");
                sb.AppendLine("========================================");
            }
        }
    }
}